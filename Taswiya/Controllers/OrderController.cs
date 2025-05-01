using ConnectChain.Features.OrderManagement.GetOrderDetails.Queries;
using ConnectChain.Features.OrderManagement.GetSupplierOrders.Queries;
using ConnectChain.Features.OrderManagement.PlaceOrder.Command;
using ConnectChain.Filters;
using ConnectChain.Helpers;
using ConnectChain.Models.Enums;
using ConnectChain.ViewModel;
using ConnectChain.ViewModel.Order.GetOrderDetails;
using ConnectChain.ViewModel.Order.GetSupplierOrder;
using ConnectChain.ViewModel.Order.PlaceOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace ConnectChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        #region GetSupplierOrders
        [HttpGet("GetSupplierOrders")]
        [Authorization(roles:Role.Supplier)]
        public async Task<IActionResult> GetSupplierOrders([FromQuery]GetSupplierOrdersRequestViewModel viewModel)
        {
            string? supplierId = Request.GetIdFromToken();
            var result = await _mediator.Send(new GetSupplierOrdersQuery(supplierId!,viewModel.OrderStatus));
            if (!result.isSuccess)
            {
                return new FailureResponseViewModel<GetSupplierOrdersResponseViewModel>(result.errorCode, result.message);
            }
            return new SuccessResponseViewModel<IReadOnlyList<GetSupplierOrdersResponseViewModel>>(result.data, result.message);
        }
        #endregion 

        #region GetOrderDetails 
        [HttpGet("GetOrderDetails")]
        [Authorization(roles:Role.Supplier)]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            var result = await _mediator.Send(new GetOrderDetailsQuery(orderId));
            if (!result.isSuccess)
            {
                return new FailureResponseViewModel<GetOrderDetailsResponseViewModel>(result.errorCode, result.message);
            }
            return new SuccessResponseViewModel<GetOrderDetailsResponseViewModel>(result.data, result.message);
        }
        #endregion

        #region Place Order

        [HttpPost("PlaceOrder")]
        [Authorization(roles: Role.Customer)]
        public async Task<ResponseViewModel<bool>> PlaceOrder(PlaceOrderRequestVeiwModel viewModel)
        {
            string? customerId = Request.GetIdFromToken();
            if (customerId == null)
            {
                return new FailureResponseViewModel<bool>(ErrorCode.UnAuthorized,"Unauthorized");
            }
            var response = await _mediator.Send(new PlaceOrderCommand(customerId,viewModel.SubTotal,viewModel.Discount,viewModel.Notes??"", [.. viewModel.Items]));
            if (!response.isSuccess)
            {
                return new FailureResponseViewModel<bool>(response.errorCode,response.message);

            }
            return new SuccessResponseViewModel<bool>(true, response.message);

        }

        #endregion
    }
}
