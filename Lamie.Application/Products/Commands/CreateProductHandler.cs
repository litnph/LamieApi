using Lamie.Domain.Entities;
using Lamie.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lamie.Application.Products.Commands
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IProductRepository _repository;

        public CreateProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // Tạo Aggregate
            var product = new Product(
                command.Sku,
                command.Price,
                command.Stock
            );

            // Nếu có SalePrice thì gọi method Domain (nếu bạn có)
            if (command.SalePrice.HasValue)
            {
                product.SetSalePrice(command.SalePrice.Value);
            }

            // Add translations
            foreach (var t in command.Translations)
            {
                product.AddTranslation(
                    t.LanguageCode,
                    t.Name,
                    t.Description
                );
            }

            // Add images
            foreach (var img in command.Images)
            {
                product.AddImage(
                    img.ImageUrl,
                    img.SortOrder
                );
            }

            // Lưu aggregate
            await _repository.AddAsync(product);

            return product.Id;
        }
    }
}
