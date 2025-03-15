using ConnectChain.Features.ImageManagement.UploadImage.Command;
using ConnectChain.Features.ProductManagement.AddProduct.Command;
using ConnectChain.Features.ProductManagement.DeleteProduct.Command;
using ConnectChain.Features.ProductManagement.GetProductById.Queries;
using ConnectChain.Features.ProductManagement.GetSupplierProducts.Queries;
using ConnectChain.Features.ProductManagement.UpdateProduct.Command;
using ConnectChain.Helpers;
using ConnectChain.ViewModel;
using ConnectChain.ViewModel.Product.AddProduct;
using ConnectChain.ViewModel.Product.GetProduct;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConnectChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;
        [AllowAnonymous]
        [HttpPost("Upload Image")]
        public async Task<ResponseViewModel<string>> UploadImage( IFormFile image)
        {
            var response = await mediator.Send(new UploadImageCommand(image));
            return response.isSuccess ? new SuccessResponseViewModel<string>(response.data)
                : new FaluireResponseViewModel<string>(response.errorCode, response.message);
        }
        [HttpGet("GetSupplierProducts")]
        public async Task<ResponseViewModel<IReadOnlyList<GetProductResponseViewModel>>> GetSupplierProducts()
        {
            var supplierId = Request.ExtractIdFromToken();
            var response = await mediator.Send(new GetSupplierProductsQuery(supplierId));
            return response.isSuccess ? new SuccessResponseViewModel<IReadOnlyList<GetProductResponseViewModel>>(response.data)
                : new FaluireResponseViewModel<IReadOnlyList<GetProductResponseViewModel>>(response.errorCode, response.message);
        }
        [HttpGet("GetSupplierProductsByPage")]
        public async Task<ResponseViewModel<IReadOnlyList<GetProductResponseViewModel>>> 
        GetSupplierProductsByPage([FromQuery] PaginationHelper paginationParams)
        {
            var supplierId = Request.ExtractIdFromToken();
            var response = await mediator.Send(new GetSupplierProductsByPageQuery(supplierId, paginationParams));
            return response.isSuccess ? new SuccessResponseViewModel<IReadOnlyList<GetProductResponseViewModel>>(response.data)
                : new FaluireResponseViewModel<IReadOnlyList<GetProductResponseViewModel>>(response.errorCode, response.message);
        }
        // [HttpGet("GetFilteredProducts")]
        /*public async Task<ResponseViewModel<IReadOnlyList<GetProductResponseViewModel>>> GetFilteredProducts([FromQuery] GetFilteredProductsRequestViewModel viewModel)
        {
        }*/
        [AllowAnonymous]
        [HttpGet("GetProductById/{productId:int}")]
        public async Task<ResponseViewModel<GetProductResponseViewModel>> GetProductById(int productId)
        {
            var response = await mediator.Send(new GetProductByIdQuery(productId));
            return response.isSuccess ? new SuccessResponseViewModel<GetProductResponseViewModel>(response.data)
                : new FaluireResponseViewModel<GetProductResponseViewModel>(response.errorCode, response.message);
        }
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
        [HttpPost("AddProductImage")]
        public async Task<ResponseViewModel<bool>> AddProductImage( [FromForm ]AddProductImageRequestViewModel viewModel)
        {
            var response = await mediator.Send(new AddProductImageCommand(viewModel.Image, viewModel.ProductId));
            return response.isSuccess ? new SuccessResponseViewModel<bool>(response.data)
                : new FaluireResponseViewModel<bool>(response.errorCode, response.message);
        }
    }
}
