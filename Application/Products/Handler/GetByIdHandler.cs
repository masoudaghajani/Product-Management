using Application.Common.Api;
using Application.Products.Queries;
using Domain.Contracts;
using Domain.IRepository;
using Domain.IRepository.ShoppingStore;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.Handler
{
    public class GetByIdHandler : IRequestHandler<GetByIdQuery, Result<Product>>, IScopedDependency
    {
        private readonly IProductRepository productRepository;

        public GetByIdHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<Result<Product>> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var info = await productRepository.GetAsync(request.Id,cancellationToken);
                if (info.Id!=Guid.Empty)
                {

                    return Result<Product>.Success(info);
                }
                return Result<Product>.Failure("API error:No Product");
            }
            catch (Exception ex)
            {
                return Result<Product>.Failure(ex.Message);
            }
        }
    }
}
