using Application.Common.Api;
using Application.Mapper;
using Application.Products.Command;
using Domain.Contracts;
using Domain.IRepository.ShoppingStore;
using Domain.Models;
using MediatR;

namespace Application.Products.Handler
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, Result<ProductResponse>>, IScopedDependency
    {
        private readonly IProductRepository productRepository;
       

        public CreateProductHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<Result<ProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                request.Id = Guid.NewGuid();
                await productRepository.AddAsync(request.ToDomain(), cancellationToken);
                return Result<ProductResponse>.Success(new ProductResponse { Id = request.Id });

            }
            catch (Exception ex)
            {

                return Result<ProductResponse>.Failure("Api Error:" + ex);

            }



        }
    }
}
