using ConnectChain.Features.WishlistManagement.AddItemToWishList.Command;
using ConnectChain.Filters;
using ConnectChain.Helpers;
using ConnectChain.Models.Enums;
using ConnectChain.ViewModel;
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
    }
}















