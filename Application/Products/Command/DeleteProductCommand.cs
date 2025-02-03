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
    public class DeleteProductCommand: IRequest<Result<ProductResponse>>, IScopedDependency
    {
        public Guid Id { get; set; }
    }
}
