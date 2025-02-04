using Application.Common.Api;
using Application.Products.Queries;
using Domain.Contracts;
using Domain.IRepository;
using Domain.IRepository.ShoppingStore;
using Domain.Models;
using MediatR;
using MediatR.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.Handler
{
    public class GetAllHandler : IRequestHandler<GetAllQuery, Result<IEnumerable<Product>>>, IScopedDependency
    {
        private readonly IProductRepository productRepository;
        public GetAllHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<Result<IEnumerable<Product>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var info = await productRepository.GetAllAsync();
                if (info.Any())
                {

                    return Result<IEnumerable<Product>>.Success(info);
                }
                return Result<IEnumerable<Product>>.Failure("API error:No Product");
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Product>>.Failure(ex.Message);
            }
        }
    }
}
