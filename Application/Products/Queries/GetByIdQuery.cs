using Domain.Contracts;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.Queries
{
    public class GetByIdQuery: IRequest<Result<Product>>, IScopedDependency
    {
        public Guid Id { get; set; }
    }
}
