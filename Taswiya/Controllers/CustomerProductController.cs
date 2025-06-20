using ConnectChain.Features.ProductManagement.GetCustomerProducts.Queries;
using ConnectChain.Filters;
using ConnectChain.Helpers;
using ConnectChain.Models.Enums;
using ConnectChain.ViewModel;
using ConnectChain.ViewModel.Product.GetCustomerProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConnectChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerProductController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("GetMatchedProducts")]
        [Authorization(Role.Customer)]
        public async Task<ResponseViewModel<IReadOnlyList<GetCustomerProductsResponseViewModel>>> GetMatchedProducts(
            )
        {
            string? customerId = Request.GetIdFromToken();
            if (customerId == null)
            {
                return new FailureResponseViewModel<IReadOnlyList<GetCustomerProductsResponseViewModel>>(ErrorCode.UnAuthorized, "Unauthorized");
            }

            var result = await _mediator.Send(new GetProductsForCustomerQuery(customerId));
            return result.isSuccess
                ? new SuccessResponseViewModel<IReadOnlyList<GetCustomerProductsResponseViewModel>>(result.data, result.message)
                : new FailureResponseViewModel<IReadOnlyList<GetCustomerProductsResponseViewModel>>(result.errorCode, result.message);
        }

        [HttpGet("GetByBusinessType")]
        public async Task<ResponseViewModel<IReadOnlyList<GetCustomerProductsResponseViewModel>>> GetByBusinessType(
            [FromQuery] string businessType,
            [FromQuery] PaginationHelper? paginationParams = null)
        {
            string? customerId = Request.GetIdFromToken(); // Optional for wishlist checking
            var result = await _mediator.Send(new GetProductsByBusinessTypeQuery(businessType, customerId, paginationParams));
            return result.isSuccess
                ? new SuccessResponseViewModel<IReadOnlyList<GetCustomerProductsResponseViewModel>>(result.data, result.message)
                : new FailureResponseViewModel<IReadOnlyList<GetCustomerProductsResponseViewModel>>(result.errorCode, result.message);
        }

        [HttpGet("Search")]
        public async Task<ResponseViewModel<IReadOnlyList<GetCustomerProductsResponseViewModel>>> Search(
            [FromQuery] string? businessType = null,
            [FromQuery] bool matchCustomerBusinessType = false,
            [FromQuery] int? categoryId = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] double? minSupplierRating = null,
            [FromQuery] bool onlyInStock = false
           )
        {
            string? customerId = Request.GetIdFromToken(); 
            var result = await _mediator.Send(new GetFilteredProductsForCustomerQuery(
                customerId, businessType, matchCustomerBusinessType, categoryId,
                minPrice, maxPrice, minSupplierRating, onlyInStock));
            return result.isSuccess
                ? new SuccessResponseViewModel<IReadOnlyList<GetCustomerProductsResponseViewModel>>(result.data, result.message)
                : new FailureResponseViewModel<IReadOnlyList<GetCustomerProductsResponseViewModel>>(result.errorCode, result.message);
        }

        [HttpGet("GetRecommendedForCustomer")]
        [Authorization(Role.Customer)]
        public async Task<ResponseViewModel<IReadOnlyList<GetCustomerProductsResponseViewModel>>> GetRecommendedForCustomer(
            [FromQuery] double minSupplierRating = 4.0,
            [FromQuery] bool onlyInStock = true
 )
        {
            string? customerId = Request.GetIdFromToken();
            if (customerId == null)
            {
                return new FailureResponseViewModel<IReadOnlyList<GetCustomerProductsResponseViewModel>>(ErrorCode.UnAuthorized, "Unauthorized");
            }

            var result = await _mediator.Send(new GetFilteredProductsForCustomerQuery(
                customerId, null, true, null, null, null, minSupplierRating, onlyInStock));
            return result.isSuccess
                ? new SuccessResponseViewModel<IReadOnlyList<GetCustomerProductsResponseViewModel>>(result.data, result.message)
                : new FailureResponseViewModel<IReadOnlyList<GetCustomerProductsResponseViewModel>>(result.errorCode, result.message);
        }
    }
}
