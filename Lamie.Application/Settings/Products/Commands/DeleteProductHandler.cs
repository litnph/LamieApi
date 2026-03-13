using Lamie.Application.Common.Exceptions;
using Lamie.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Lamie.Application.Settings.Products.Commands
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _repository;

        public DeleteProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);
            if (product is null)
            {
                throw new NotFoundException("Product", request.Id);
            }

            await _repository.DeleteAsync(product);
        }
    }
}

