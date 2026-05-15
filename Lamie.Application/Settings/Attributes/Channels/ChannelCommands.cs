using FluentValidation;
using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Entities;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.Settings.Attributes.Channels;

public sealed record CreateChannelCommand(string Code, string Name, string? IconUrl, int SortOrder, bool IsActive) : IRequest<Guid>;

public sealed record UpdateChannelCommand(Guid Id, string Name, string? IconUrl, int SortOrder, bool IsActive) : IRequest;

public sealed record DeleteChannelCommand(Guid Id) : IRequest;

public sealed class CreateChannelValidator : AbstractValidator<CreateChannelCommand>
{
    public CreateChannelValidator()
    {
        RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.IconUrl).MaximumLength(500);
        RuleFor(x => x.SortOrder).GreaterThanOrEqualTo(0);
    }
}

public sealed class UpdateChannelValidator : AbstractValidator<UpdateChannelCommand>
{
    public UpdateChannelValidator()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.IconUrl).MaximumLength(500);
        RuleFor(x => x.SortOrder).GreaterThanOrEqualTo(0);
    }
}

public sealed class CreateChannelHandler : IRequestHandler<CreateChannelCommand, Guid>
{
    private readonly IChannelRepository _repository;

    public CreateChannelHandler(IChannelRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateChannelCommand request, CancellationToken cancellationToken)
    {
        var normalized = request.Code.Trim().ToUpperInvariant();
        if (await _repository.ExistsCodeAsync(normalized, null, cancellationToken))
        {
            throw new ConflictException($"Channel code '{normalized}' already exists.");
        }

        var channel = new Channel(normalized, request.Name, request.IconUrl, request.SortOrder, request.IsActive);
        await _repository.AddAsync(channel, cancellationToken);
        return channel.Id;
    }
}

public sealed class UpdateChannelHandler : IRequestHandler<UpdateChannelCommand>
{
    private readonly IChannelRepository _repository;

    public UpdateChannelHandler(IChannelRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateChannelCommand request, CancellationToken cancellationToken)
    {
        var channel = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Channel", request.Id);

        channel.Update(request.Name, request.IconUrl, request.SortOrder, request.IsActive);
        await _repository.UpdateAsync(channel, cancellationToken);
    }
}

public sealed class DeleteChannelHandler : IRequestHandler<DeleteChannelCommand>
{
    private readonly IChannelRepository _repository;

    public DeleteChannelHandler(IChannelRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteChannelCommand request, CancellationToken cancellationToken)
    {
        var channel = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Channel", request.Id);

        await _repository.DeleteAsync(channel, cancellationToken);
    }
}
