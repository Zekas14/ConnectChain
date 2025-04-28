using ConnectChain.Features.ProductManagement.Products.DeleteProduct;
using ConnectChain.Features.ProductManagement.Products.DeleteProduct.Command;
using ConnectChain.Features.ProductManagement.Products.GetFilteredProducts.Queries;
using ConnectChain.Features.ProductManagement.Products.GetProductDetails.Queries;
using ConnectChain.Features.ProductManagement.Products.GetProductForUpdate;
using ConnectChain.Features.ProductManagement.Products.GetSupplierProducts.Queries;
using ConnectChain.Features.ProductManagement.Products.UpdateProduct.Command;
using ConnectChain.Features.ProductManagement.Products.SearchProduct.Queries;
using ConnectChain.Filters;
using ConnectChain.Helpers;
using ConnectChain.Models.Enums;
using ConnectChain.ViewModel;
using ConnectChain.ViewModel.Product.AddProduct;
using ConnectChain.ViewModel.Product.GetFilteredProducts;
using ConnectChain.ViewModel.Product.GetProductDetails;
using ConnectChain.ViewModel.Product.GetProductForUpdate;
using ConnectChain.ViewModel.Product.GetSupplierProduct;
using ConnectChain.ViewModel.Product.SearchProduct;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ConnectChain.Features.ProductManagement.Products.AddProduct.Commands;

namespace ConnectChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    [Authorization(Role.Supplier)]
    public class ProductController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;

        #region SearchForProduct
        [HttpGet("SearchForProducts")]
        [AllowAnonymous]
        public async Task<ResponseViewModel<IReadOnlyList<GetSupplierProductResponseViewModel>>> SearchForProduct(SearchProductRequestViewModel viewModel)
        {
            var response = await mediator.Send(new SearchProductQuery(viewModel.SupplierId, viewModel.SearchKey));
            return response.isSuccess ? new SuccessResponseViewModel<IReadOnlyList<GetSupplierProductResponseViewModel>>(response.data)
            : new FailureResponseViewModel<IReadOnlyList<GetSupplierProductResponseViewModel>>(response.errorCode, response.message);
        }
        #endregion
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
            return response.isSuccess ? new SuccessResponseViewModel<IReadOnlyList<GetSupplierProductResponseViewModel>>(response.data)
                : new FailureResponseViewModel<IReadOnlyList<GetSupplierProductResponseViewModel>>(response.errorCode, response.message);
        }
        #endregion

        #region GetSupplierProductsByPage
        [HttpGet("GetSupplierProductsByPage")]
        public async Task<IActionResult> 
        GetSupplierProductsByPage([FromQuery] PaginationHelper paginationParams)
        {
            var supplierId = Request.GetIdFromToken();
            var response = await mediator.Send(new GetSupplierProductsByPageQuery(supplierId, paginationParams));
            return response.isSuccess ? new SuccessResponseViewModel<IReadOnlyList<GetSupplierProductResponseViewModel>>(response.data)
                : new FailureResponseViewModel<IReadOnlyList<GetSupplierProductResponseViewModel>>(response.errorCode, response.message);
        }
        #endregion

        #region GetFilteredProducts 
        [HttpGet("GetFilteredProducts")]
        
        public async Task<IActionResult> GetFilteredProducts(GetFilteredProductsRequestViewModel viewModel)
        {
            var response = await mediator.Send(new GetFilteredProductsQuery(viewModel.Filters));
            return response.isSuccess ? new SuccessResponseViewModel<IReadOnlyList<GetSupplierProductResponseViewModel>>(response.data)
                : new FailureResponseViewModel<IReadOnlyList<GetSupplierProductResponseViewModel>>(response.errorCode, response.message);
        }
        #endregion

        #region Get Product Details
        [HttpGet("GetProductDetails")]
        public async Task<IActionResult> GetProductDetails([FromQuery] int productId)
        {
            var response = await mediator.Send(new GetProductDetailsQuery(productId));
            return response.isSuccess ? 
                new SuccessResponseViewModel<ConnectChain.ViewModel.Product.GetProductDetails.GetProductDetailsResponseViewModel >(response.data)
                : new FailureResponseViewModel<GetProductDetailsResponseViewModel>(response.errorCode, response.message);
        }
        #endregion

        #region Add Product
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromForm]AddProductRequestViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
              var supplierId = Request.GetIdFromToken();

                var response = await mediator.Send(new AddProductCommand
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    Images = viewModel.Images,
                    MinimumStock = viewModel.MinimumStock,
                    Price = viewModel.Price,
                    Stock = viewModel.Stock,
                    SupplierId = supplierId,
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
        public async Task<IActionResult> UpdateProduct(int productId,[FromForm] UpdateProductRequestViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await mediator.Send(new UpdateProductCommand
                {
                    ProductId = productId,
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    MinimumStock = viewModel.MinimumStock,
                    Images = viewModel.Images,
                    Price = viewModel.Price,
                    Stock = viewModel.Stock,
                    CategoryId = viewModel.CategoryId
                });
                return response.isSuccess ? new SuccessResponseViewModel<bool>(response.data,response.message)
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

        #region Get Product For Update
        [HttpGet("GetProductForUpdate")]
        public async Task<IActionResult> GetProductForUpdate([FromQuery]int productId)
        {
            var response = await mediator.Send(new GetProductForUpdateQuery(productId));
            return response.isSuccess ? new SuccessResponseViewModel<GetProductForUpdateResponseViewModel>(response.data)
                : new FailureResponseViewModel<GetProductForUpdateResponseViewModel>(response.errorCode, response.message);
        }
        #endregion

        #region Delete Product Image
        [HttpDelete("DeleteProductImage")]
        public async Task<IActionResult> DeleteProductImage(int id)
        {
            var response = await mediator.Send(new DeleteProdcutImageCommand(id));
            return response.isSuccess ? new SuccessResponseViewModel<bool>(response.data,response.message)
                : new FailureResponseViewModel<bool>(response.errorCode, response.message);
        }
        #endregion
    }
}
