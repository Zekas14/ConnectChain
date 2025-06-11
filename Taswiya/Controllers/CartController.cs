using Azure;
using ConnectChain.Features.CartManagement.Cart.Commands.AddToCart.Command;
using ConnectChain.Features.CartManagement.Cart.Commands.ClearCart;
using ConnectChain.Features.CartManagement.Cart.Commands.RemoveFromCart;
using ConnectChain.Features.CartManagement.Cart.Commands.UpdateCartItem;
using ConnectChain.Features.CartManagement.Cart.Queries.GetCart.Query;
using ConnectChain.Filters;
using ConnectChain.Helpers;
using ConnectChain.Models.Enums;
using ConnectChain.ViewModel;
using ConnectChain.ViewModel.Cart.AddToCart;
using ConnectChain.ViewModel.Cart.GetCartItems;
using ConnectChain.ViewModel.Cart.UpdateCartItem;
using ConnectChain.ViewModel.Category.GetCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConnectChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorization(Role.Customer)]
    public class CartController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;
        [HttpGet("GetCartItems")]
        public async Task<ResponseViewModel<GetCartItemsResponseViewModel>> GetCart()
        {
            string? customerId = Request.GetIdFromToken();
            var result = await mediator.Send(new GetCartQuery(customerId!));
            return result.isSuccess ?
                new SuccessResponseViewModel<GetCartItemsResponseViewModel>(result.data,result.message):
                new FailureResponseViewModel<GetCartItemsResponseViewModel>(result.errorCode,result.message);
                ;
        }
        [HttpPost("AddItemToCart")]
        public async Task<ResponseViewModel<bool>> AddToCart(AddToCartRequestViewModel viewModel,CancellationToken cancellationToken)
        {
            string? customerId = Request.GetIdFromToken();
           var response = await mediator.Send(new AddToCartCommand(customerId!,viewModel.ProductId,viewModel.Quantity),cancellationToken);
            return response.isSuccess ? new SuccessResponseViewModel<bool>(true,response.message):
                new  FailureResponseViewModel<bool>(response.errorCode,response.message);
        }
        [HttpDelete("RemoveFromCart")]
        public async Task<ResponseViewModel<bool>> RemoveFromCart(int itemId)
        {
            string? customerId = Request.GetIdFromToken();
            var response = await mediator.Send(new RemoveCartItemCommand(customerId!,itemId));
            return response.isSuccess ? new SuccessResponseViewModel<bool>(true, response.message) :
            new FailureResponseViewModel<bool>(response.errorCode, response.message);

        }
        [HttpDelete("ClearCart")]
        public async Task<ResponseViewModel<bool>> ClearCart()
        {
            string? customerId = Request.GetIdFromToken();
            var response = await mediator.Send(new ClearCartCommand(customerId!));
            return response.isSuccess ? new SuccessResponseViewModel<bool>(true, response.message) :
            new FailureResponseViewModel<bool>(response.errorCode, response.message);

        }
        [HttpPut("UpdateCartItem")]
        public async Task<ResponseViewModel<bool>> RemoveFromCart(UpdateCartRequestViewModel viewModel)
        {
            string? customerId = Request.GetIdFromToken();
            var response = await mediator.Send(new UpdateCartItemCommand(customerId!,viewModel.ItemId,viewModel.Quantity));
            return response.isSuccess ? new SuccessResponseViewModel<bool>(true, response.message) :
            new FailureResponseViewModel<bool>(response.errorCode, response.message);

        }
    }
}
