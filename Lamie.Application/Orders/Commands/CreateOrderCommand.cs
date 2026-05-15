using FluentValidation;
using Lamie.Application.Common.Exceptions;
using Lamie.Application.Common.Storage;
using ValidationException = Lamie.Application.Common.Exceptions.ValidationException;
using Lamie.Application.Orders.Dtos;
using Lamie.Domain.Entities.Orders;
using Lamie.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Lamie.Application.Orders.Commands;

public sealed class CreateOrderItemInput
{
    public Guid? ProductId { get; set; }
    public string? ProductSku { get; set; }
    public string ProductName { get; set; } = default!;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public string? Note { get; set; }
}

public sealed class CreateOrderImageInput
{
    public string? ImageUrl { get; set; }
    public int? SortOrder { get; set; }
    public string? Description { get; set; }
    public IFormFile? ImageFile { get; set; }
}

public sealed class CreateOrderCommand : IRequest<OrderDetailDto>
{
    public string OrdererName { get; set; } = default!;
    public string OrdererPhone { get; set; } = default!;
    public Guid ChannelId { get; set; }
    public string RecipientName { get; set; } = default!;
    public string RecipientPhone { get; set; } = default!;
    public bool PickupAtShop { get; set; }
    public string? DeliveryAddress { get; set; }
    public double? DeliveryLatitude { get; set; }
    public double? DeliveryLongitude { get; set; }
    public DateTime DeliveryAt { get; set; }
    public decimal DepositAmount { get; set; }
    public decimal ShippingFee { get; set; }
    public string? Description { get; set; }
    public string? ContentNote { get; set; }
    public List<CreateOrderItemInput> Items { get; set; } = new();
    public List<CreateOrderImageInput> Images { get; set; } = new();
}

public sealed class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.OrdererName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.OrdererPhone).NotEmpty().MaximumLength(30);
        RuleFor(x => x.ChannelId).NotEqual(Guid.Empty);
        RuleFor(x => x.RecipientName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.RecipientPhone).NotEmpty().MaximumLength(30);
        RuleFor(x => x.DeliveryAt).NotEqual(default(DateTime));
        RuleFor(x => x.DepositAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ShippingFee).GreaterThanOrEqualTo(0);

        When(x => !x.PickupAtShop, () =>
        {
            RuleFor(x => x.DeliveryAddress).NotEmpty()
                .WithMessage("DeliveryAddress is required when not picking up at shop");
        });

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductName).NotEmpty().MaximumLength(255);
            item.RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
            item.RuleFor(x => x.Quantity).GreaterThan(0);
        });

        RuleForEach(x => x.Images).ChildRules(img =>
        {
            img.RuleFor(x => x)
                .Must(i => !string.IsNullOrWhiteSpace(i.ImageUrl) || (i.ImageFile is { Length: > 0 }))
                .WithMessage("Either ImageUrl or ImageFile is required");
        });
    }
}

public sealed class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderDetailDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IChannelRepository _channelRepository;
    private readonly IFileStorage _fileStorage;

    public CreateOrderHandler(
        IOrderRepository orderRepository,
        IChannelRepository channelRepository,
        IFileStorage fileStorage)
    {
        _orderRepository = orderRepository;
        _channelRepository = channelRepository;
        _fileStorage = fileStorage;
    }

    public async Task<OrderDetailDto> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.GetByIdAsync(command.ChannelId, cancellationToken)
            ?? throw new ValidationException(new() { ["channelId"] = ["Channel does not exist"] });

        var orderCode = await GenerateOrderCodeAsync(command.DeliveryAt, cancellationToken);

        var order = new Order(
            orderCode,
            command.OrdererName,
            command.OrdererPhone,
            channel.Id,
            command.RecipientName,
            command.RecipientPhone,
            command.PickupAtShop,
            command.DeliveryAddress,
            command.DeliveryLatitude,
            command.DeliveryLongitude,
            command.DeliveryAt,
            command.DepositAmount,
            command.ShippingFee,
            command.Description,
            command.ContentNote);

        foreach (var item in command.Items)
        {
            order.AddItem(item.ProductId, item.ProductSku, item.ProductName, item.UnitPrice, item.Quantity, item.Note);
        }

        await UploadImagesAsync(order, command.Images, cancellationToken);

        await _orderRepository.AddAsync(order, cancellationToken);

        var logs = await _orderRepository.GetChangeLogsAsync(order.Id, cancellationToken);
        return OrderMapper.ToDetail(order, logs);
    }

    private async Task<string> GenerateOrderCodeAsync(DateTime deliveryAt, CancellationToken cancellationToken)
    {
        var prefix = $"ORD-{DateTime.UtcNow:yyyyMMdd}-";
        var count = await _orderRepository.CountByDatePrefixAsync(prefix, cancellationToken);
        return $"{prefix}{(count + 1):D4}";
    }

    private async Task UploadImagesAsync(Order order, List<CreateOrderImageInput> images, CancellationToken cancellationToken)
    {
        if (images is null || images.Count == 0) return;

        for (var i = 0; i < images.Count; i++)
        {
            var image = images[i];
            var sort = image.SortOrder ?? i;

            if (image.ImageFile is { Length: > 0 })
            {
                var path = OrderObjectPath.Build(order.OrderCode, image.ImageFile.FileName, i);
                await using var stream = image.ImageFile.OpenReadStream();
                var url = await _fileStorage.UploadPublicAsync(
                    stream,
                    path,
                    image.ImageFile.ContentType ?? "application/octet-stream",
                    cancellationToken);
                order.AddImage(url, sort, image.Description);
            }
            else if (!string.IsNullOrWhiteSpace(image.ImageUrl))
            {
                order.AddImage(image.ImageUrl, sort, image.Description);
            }
        }
    }
}

internal static class OrderObjectPath
{
    public static string Build(string orderCode, string? fileName, int index)
    {
        var safeOrder = string.IsNullOrWhiteSpace(orderCode) ? "unknown" : Sanitize(orderCode);
        var name = string.IsNullOrWhiteSpace(fileName) ? $"image-{index}" : fileName;

        var safeName = Sanitize(Path.GetFileName(name));
        var ext = Path.GetExtension(safeName);
        var nameNoExt = Path.GetFileNameWithoutExtension(safeName);
        var stamp = Guid.NewGuid().ToString("N");

        var final = string.IsNullOrWhiteSpace(ext)
            ? $"{nameNoExt}-{stamp}"
            : $"{nameNoExt}-{stamp}{ext}";

        return $"orders/{safeOrder}/{final}";
    }

    private static string Sanitize(string value)
    {
        var cleaned = value.Trim().Replace(' ', '-').Replace('\\', '-').Replace('/', '-');
        return string.IsNullOrWhiteSpace(cleaned) ? "x" : cleaned;
    }
}
