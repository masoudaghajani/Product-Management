using Application.Common.Api;
using Domain.Contracts;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.Command
{
    public class UpdateProductCommand: IRequest<Result<ProductResponse>>, IScopedDependency
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; } = 0;
    }
}
