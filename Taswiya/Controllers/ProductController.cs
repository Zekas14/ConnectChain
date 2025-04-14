using ConnectChain.Features.ImageManagement.UploadImage.Command;
using ConnectChain.Features.ProductManagement.AddProduct.Command;
using ConnectChain.Features.ProductManagement.DeleteProduct.Command;
using ConnectChain.Features.ProductManagement.GetFilteredProducts.Queries;
using ConnectChain.Features.ProductManagement.GetProductById.Queries;
using ConnectChain.Features.ProductManagement.GetSupplierProducts.Queries;
using ConnectChain.Features.ProductManagement.UpdateProduct.Command;
using ConnectChain.Helpers;
using ConnectChain.ViewModel;
using ConnectChain.ViewModel.Product.AddProduct;
using ConnectChain.ViewModel.Product.GetFilteredProducts;
using ConnectChain.ViewModel.Product.GetProduct;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConnectChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProductController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;

        #region GetSupplierProducts
        [HttpGet("GetSupplierProducts")]
        public async Task<IActionResult> GetSupplierProducts(string supplierId)
        {
           /* var supplierId = Request.ExtractIdFromToken();
            if (string.IsNullOrEmpty(supplierId))
            {
                return new FaluireResponseViewModel<IReadOnlyList<GetProductResponseViewModel>>(ErrorCode.UnAuthorized, ErrorCode.UnAuthorized.ToString());
            }*/
            var response = await mediator.Send(new GetSupplierProductsQuery(supplierId));
            return response.isSuccess ? new SuccessResponseViewModel<IReadOnlyList<GetProductResponseViewModel>>(response.data)
                : new FailureResponseViewModel<IReadOnlyList<GetProductResponseViewModel>>(response.errorCode, response.message);
        }
        #endregion

        #region GetSupplierProductsByPage
        [HttpGet("GetSupplierProductsByPage")]
        public async Task<IActionResult> 
        GetSupplierProductsByPage([FromQuery] PaginationHelper paginationParams)
        {
            var supplierId = Request.ExtractIdFromToken();
            var response = await mediator.Send(new GetSupplierProductsByPageQuery(supplierId, paginationParams));
            return response.isSuccess ? new SuccessResponseViewModel<IReadOnlyList<GetProductResponseViewModel>>(response.data)
                : new FailureResponseViewModel<IReadOnlyList<GetProductResponseViewModel>>(response.errorCode, response.message);
        }
        #endregion

        #region GetFilteredProducts 
        [HttpGet("GetFilteredProducts")]
        public async Task<IActionResult> GetFilteredProducts([FromQuery] GetFilteredProductsRequestViewModel viewModel)
        {
            var response = await mediator.Send(new GetFilteredProductsQuery(viewModel.PageNumber,viewModel.PageSize,viewModel.Filters));
            return response.isSuccess ? new SuccessResponseViewModel<IReadOnlyList<GetProductResponseViewModel>>(response.data)
                : new FailureResponseViewModel<IReadOnlyList<GetProductResponseViewModel>>(response.errorCode, response.message);
        }
        #endregion

        #region GetProductById
        [AllowAnonymous]
        [HttpGet("GetProductById/{productId:int}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var response = await mediator.Send(new GetProductByIdQuery(productId));
            return response.isSuccess ? new SuccessResponseViewModel<GetProductResponseViewModel>(response.data)
                : new FailureResponseViewModel<GetProductResponseViewModel>(response.errorCode, response.message);
        }
        #endregion

        #region Add Product
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct(AddProductRequestViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
              //  var supplierId = Request.ExtractIdFromToken();

                var response = await mediator.Send(new AddProductCommand
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    Price = viewModel.Price,
                    Stock = viewModel.Stock,
                    SupplierId = viewModel.SupplierId,
                    CategoryId = viewModel.CategoryId
                });
                return response.isSuccess ? new SuccessResponseViewModel<bool>(response.data, response.message)
                    : new FailureResponseViewModel<bool>(response.errorCode, response.message);

            }
            var errors = string.Join(", ", ModelState.Select(e => e.Value!.Errors));
            return new FailureResponseViewModel<bool>(ErrorCode.BadRequest,errors);
        }
        #endregion

        #region Delete Product

        [HttpDelete("DeleteProduct/{productId:int}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var response = await mediator.Send(new DeleteProductCommand(productId));
            return response.isSuccess ? new SuccessResponseViewModel<bool>(response.data)
                : new FailureResponseViewModel<bool>(response.errorCode, response.message);
        }
        #endregion

        #region Update Product
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromRoute]int productId,[FromBody]UpdateProductRequestViewModel viewModel)
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
                    : new FailureResponseViewModel<bool>(response.errorCode, response.message);
            }
            var errors = string.Join(", ", ModelState.Select(e => e.Value!.Errors));
            return  FailureResponseViewModel<bool>.BadRequest(errors);
        }
        #endregion

        #region Add Product Image
        [HttpPost("AddProductImage")]
        public async Task<IActionResult> AddProductImage( [FromForm ]AddProductImageRequestViewModel viewModel)
        {
            var response = await mediator.Send(new AddProductImageCommand(viewModel.Image, viewModel.ProductId));
            return response.isSuccess ? new SuccessResponseViewModel<bool>(response.data)
                : new FailureResponseViewModel<bool>(response.errorCode, response.message);
        }
        #endregion
    }
}
