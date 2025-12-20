using ConnectChain.Features.WishlistManagement.AddItemToWishList.Command;
using ConnectChain.Features.WishlistManagement.GetWishlistProducts.Queries;
using ConnectChain.Features.WishlistManagement.RemoveItemFromWishList.Command;
using ConnectChain.Filters;
using ConnectChain.Helpers;
using ConnectChain.Models.Enums;
using ConnectChain.ViewModel;
using ConnectChain.ViewModel.Wishlist.GetWishlistProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace ConnectChain.Controllers
{
    [Route("api/[controller]")]  
    [ApiController]
    [Authorization(Role.Customer)]   
    public class WishlistController(IMediator mediator) : ControllerBase
    { 
        private readonly IMediator _mediator = mediator;
        [HttpPost("AddToWishlist")]
        public async Task<ResponseViewModel<bool>> AddToWishlist(int productId)
        {
            string? customerId = Request.GetIdFromToken();
            var result = await _mediator.Send(new AddToWishItemCommand(productId, customerId!));
            return result.isSuccess
             ? new SuccessResponseViewModel<bool>(true, result.message)
             : new FailureResponseViewModel<bool>(result.errorCode, result.message);
        }

        [HttpDelete("RemoveFromWishlist")]
        public async Task<ResponseViewModel<bool>> RemoveFromWishlist(int productId)
        {
            string? customerId = Request.GetIdFromToken();
            var result = await _mediator.Send(new RemoveFromWishItemCommand(productId, customerId!));
            return result.isSuccess
             ? new SuccessResponseViewModel<bool>(true, result.message)
             : new FailureResponseViewModel<bool>(result.errorCode, result.message);
        }

        [HttpGet("GetWishlistProducts")]
        public async Task<ResponseViewModel<IReadOnlyList<GetWishlistProductsResponseViewModel>>> GetWishlistProducts()
        {
            string? customerId = Request.GetIdFromToken();
            var result = await _mediator.Send(new GetWishlistProductsQuery(customerId!));
            return result.isSuccess
             ? new SuccessResponseViewModel<IReadOnlyList<GetWishlistProductsResponseViewModel>>(result.data, result.message)
             : new FailureResponseViewModel<IReadOnlyList<GetWishlistProductsResponseViewModel>>(result.errorCode, result.message);
        }

        [HttpGet("GetWishlistProductsByPage")]
        public async Task<ResponseViewModel<IReadOnlyList<GetWishlistProductsResponseViewModel>>> GetWishlistProductsByPage([FromQuery] PaginationHelper paginationParams)
        {
            string? customerId = Request.GetIdFromToken();
            var result = await _mediator.Send(new GetWishlistProductsByPageQuery(customerId!, paginationParams));
            return result.isSuccess
             ? new SuccessResponseViewModel<IReadOnlyList<GetWishlistProductsResponseViewModel>>(result.data, result.message)
             : new FailureResponseViewModel<IReadOnlyList<GetWishlistProductsResponseViewModel>>(result.errorCode, result.message);
        }
    }
}















