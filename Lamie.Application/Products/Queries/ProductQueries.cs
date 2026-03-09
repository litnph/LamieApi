using Lamie.Application.Products.Dtos;
using Lamie.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http.Features.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lamie.Application.Products.Queries
{
    public class GetAllProductsQuery : IRequest<List<Product>>
    {
    }

    public class GetProductByIdQuery : IRequest<Product>
    {
        public int Id { get; set; }

        public GetProductByIdQuery(int id)
        {
            Id = id;
        }
    }
}
