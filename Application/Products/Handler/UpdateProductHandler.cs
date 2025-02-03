using Application.Mapper;
using Application.Products.Command;
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
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Result<ProductResponse>>, IScopedDependency
    {
        private readonly IProductRepository productRepository;
        public UpdateProductHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<Result<ProductResponse>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                
                await productRepository.UpdateAsync(request.ToDomain());
                return Result<ProductResponse>.Success(new ProductResponse { Id = request.Id });

            }
            catch (Exception ex)
            {

                return Result<ProductResponse>.Failure("Api Error:" + ex);

            }
        }
    }
}
