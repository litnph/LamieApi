using Lamie.Application.Common.Exceptions;
using Lamie.Application.Products.Dtos;
using Lamie.Domain.Entities;
using Lamie.Domain.Repositories;
using MediatR;

namespace Lamie.Application.Products.Queries
{
    public class GetAllProductsHandler
    : IRequestHandler<GetAllProductsQuery, List<Product>>
    {
        private readonly IProductRepository _repository;

        public GetAllProductsHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Product>> Handle(
            GetAllProductsQuery request,
            CancellationToken cancellationToken)
        {
            var products = await _repository.GetAllAsync();
            return products?.ToList() ?? new List<Product>();
        }
    }
    public class GetProductByIdHandler
        : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly IProductRepository _repository;

        public GetProductByIdHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Product> Handle(
            GetProductByIdQuery request,
            CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);
            if(product == null)
                throw new NotFoundException("Product", request.Id);

            return product;
        }
    }
}
