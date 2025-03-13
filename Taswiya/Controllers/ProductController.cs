using ConnectChain.Features.ProductManagement.AddProduct.Command;
using ConnectChain.Features.ProductManagement.DeleteProduct.Command;
using ConnectChain.Features.ProductManagement.UpdateProduct.Command;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel;
using ConnectChain.ViewModel.Product.AddProduct;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConnectChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;
        [HttpPost("AddProduct")]
        public async Task<ResponseViewModel<bool>> AddProduct(AddProductRequestViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var supplierId = Request.ExtractIdFromToken();

                var response = await mediator.Send(new AddProductCommand
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    Image = viewModel.Image,
                    Price = viewModel.Price,
                    Stock = viewModel.Stock,
                    SupplierId = supplierId,
                    CategoryId = viewModel.CategoryId
                });
                return response.isSuccess ? new SuccessResponseViewModel<bool>(response.data)
                    : new FaluireResponseViewModel<bool>(response.errorCode, response.message);

            }
            var errors = string.Join(", ", ModelState.Select(e => e.Value!.Errors));
            return new FaluireResponseViewModel<bool>(ErrorCode.BadRequest,errors);
        }
        [HttpDelete("DeleteProduct/{productId:int}")]
        public async Task<ResponseViewModel<bool>> DeleteProduct(int productId)
        {
            var response = await mediator.Send(new DeleteProductCommand(productId));
            return response.isSuccess ? new SuccessResponseViewModel<bool>(response.data)
                : new FaluireResponseViewModel<bool>(response.errorCode, response.message);
        }
        [HttpPut("UpdateProduct")]
        public async Task<ResponseViewModel<bool>> UpdateProduct([FromRoute]int productId,[FromBody]UpdateProductRequestViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await mediator.Send(new UpdateProductCommand
                {
                    ProductId = productId,
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    Image = viewModel.Image,
                    Price = viewModel.Price,
                    Stock = viewModel.Stock,
                    CategoryId = viewModel.CategoryId
                });
                return response.isSuccess ? new SuccessResponseViewModel<bool>(response.data)
                    : new FaluireResponseViewModel<bool>(response.errorCode, response.message);
            }
            var errors = string.Join(", ", ModelState.Select(e => e.Value!.Errors));
            return  FaluireResponseViewModel<bool>.BadRequest(errors);
        }
    }
}
