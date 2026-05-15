using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.Settings.Products.Commands;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _repository;

    public DeleteProductHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Product", request.Id);

        await _repository.DeleteAsync(product, cancellationToken);
    }
}
