using Application.Products.Command;
using Application.Products.Queries;
using Domain.Models;
using Infrastructure.Api;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Presentation.Filter;
using Presentation.ViewModels;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableRateLimiting("SlowDown")]
    [ServiceFilter(typeof(ExceptionAttribute))]
    [Authorize]
    public class HomeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

       
        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            var query = new GetAllQuery();
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(ApiResult<IEnumerable<Product>>.Success(result.Value));
            }

            return BadRequest(ApiResult<IEnumerable<Product>>.Failure(result.Error));
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetByIdQuery{ Id=id};
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(ApiResult<Product>.Success(result.Value));
            }

            return BadRequest(ApiResult<Product>.Failure(result.Error));
        }
        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct(ProductAddVm productAddVm)
        {
            var query = new CreateProductCommand { Code = productAddVm.Code, Name = productAddVm.Name, Price = productAddVm.Price, StockQuantity = productAddVm.StockQuantity };
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(ApiResult<Guid>.Success(result.Value.Id));
            }

            return BadRequest(ApiResult<IEnumerable<Product>>.Failure(result.Error));
        }
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(ProductAddVm productAddVm)
        {
            var query = new UpdateProductCommand { Code = productAddVm.Code, Name = productAddVm.Name, Price = productAddVm.Price, StockQuantity = productAddVm.StockQuantity };
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(ApiResult<Guid>.Success(result.Value.Id));
            }

            return BadRequest(ApiResult<IEnumerable<Product>>.Failure(result.Error));
        }
        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(Guid productGuid)
        {
            var query = new DeleteProductCommand { Id= productGuid };
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(ApiResult<Guid>.Success(result.Value.Id));
            }

            return BadRequest(ApiResult<IEnumerable<Product>>.Failure(result.Error));
        }

    }
}
