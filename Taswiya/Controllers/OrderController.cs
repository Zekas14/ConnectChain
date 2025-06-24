using ConnectChain.Features.OrderManagement.GetCheckOutSummary.Query;
using ConnectChain.Features.OrderManagement.GetCustomerOrders.Queries;
using ConnectChain.Features.OrderManagement.GetOrderDetails.Queries;
using ConnectChain.Features.OrderManagement.GetSupplierOrders.Queries;
using ConnectChain.Features.OrderManagement.PlaceOrder.Command;
using ConnectChain.Features.OrderManagement.CancelOrder.Commands;
using ConnectChain.Features.OrderManagement.CancelOrder.Queries;
using ConnectChain.Filters;
using ConnectChain.Helpers;
using ConnectChain.Models.Enums;
using ConnectChain.ViewModel;
using ConnectChain.ViewModel.Order.GetCheckOutSummary;
using ConnectChain.ViewModel.Order.GetCustomerOrders;
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
        public async Task<ResponseViewModel<bool>> PlaceOrder(PlaceOrderRequestViewModel viewModel)
        {
            string? customerId = Request.GetIdFromToken();
            if (customerId == null)
            {
                return new FailureResponseViewModel<bool>(ErrorCode.UnAuthorized,"Unauthorized");
            }
            var response = await _mediator.Send(new PlaceOrderCommand(customerId,viewModel.Discount,viewModel.Notes??""));
            if (!response.isSuccess)
            {
                return new FailureResponseViewModel<bool>(response.errorCode,response.message);

            }
            return new SuccessResponseViewModel<bool>(true, response.message);

        }

        #endregion

        #region GetCheckOutSummary 
        [HttpGet("GetCheckOutSummary")]
        [Authorization(Role.Customer)]
        public async Task<ResponseViewModel<CheckoutSummaryResponseViewModel>> GetCheckOutSummary()
        {
            var customerId = Request.GetIdFromToken();
            if (customerId == null)
            {
                return new FailureResponseViewModel<CheckoutSummaryResponseViewModel>(ErrorCode.UnAuthorized, "Unauthorized");
            }
            var response = await _mediator.Send(new GetCheckoutSummaryQuery(customerId));
            return response.isSuccess ?
                  new SuccessResponseViewModel<CheckoutSummaryResponseViewModel>(response.data, response.message) :
                  new FailureResponseViewModel<CheckoutSummaryResponseViewModel>(response.errorCode,response.message);

        }
        #endregion

        #region GetCustomerOrders
        [HttpGet("GetCustomerOrders")]
        [Authorization(Role.Customer)]
        public async Task<ResponseViewModel<IReadOnlyList<GetCustomerOrdersResponseViewModel>>> GetCustomerOrders()
        {
            string? customerId = Request.GetIdFromToken();
            if (customerId == null)
            {
                return new FailureResponseViewModel<IReadOnlyList<GetCustomerOrdersResponseViewModel>>(ErrorCode.UnAuthorized, "Unauthorized");
            }

            var result = await _mediator.Send(new GetCustomerOrdersQuery(customerId));
            return result.isSuccess
                ? new SuccessResponseViewModel<IReadOnlyList<GetCustomerOrdersResponseViewModel>>(result.data, result.message)
                : new FailureResponseViewModel<IReadOnlyList<GetCustomerOrdersResponseViewModel>>(result.errorCode, result.message);
        }

        [HttpGet("GetCustomerOrdersByPage")]
        [Authorization(Role.Customer)]
        public async Task<ResponseViewModel<IReadOnlyList<GetCustomerOrdersResponseViewModel>>> GetCustomerOrdersByPage([FromQuery] PaginationHelper paginationParams)
        {
            string? customerId = Request.GetIdFromToken();
            if (customerId == null)
            {
                return new FailureResponseViewModel<IReadOnlyList<GetCustomerOrdersResponseViewModel>>(ErrorCode.UnAuthorized, "Unauthorized");
            }

            var result = await _mediator.Send(new GetCustomerOrdersByPageQuery(customerId, paginationParams));
            return result.isSuccess
                ? new SuccessResponseViewModel<IReadOnlyList<GetCustomerOrdersResponseViewModel>>(result.data, result.message)
                : new FailureResponseViewModel<IReadOnlyList<GetCustomerOrdersResponseViewModel>>(result.errorCode, result.message);
        }

        [HttpGet("GetCustomerOrderByNumber")]
        [Authorization(Role.Customer)]
        public async Task<ResponseViewModel<GetCustomerOrdersResponseViewModel>> GetCustomerOrderByNumber(string orderNumber)
        {
            string? customerId = Request.GetIdFromToken();
            if (customerId == null)
            {
                return new FailureResponseViewModel<GetCustomerOrdersResponseViewModel>(ErrorCode.UnAuthorized, "Unauthorized");
            }

            var result = await _mediator.Send(new GetCustomerOrderByNumberQuery(customerId, orderNumber));
            return result.isSuccess
                ? new SuccessResponseViewModel<GetCustomerOrdersResponseViewModel>(result.data, result.message)
                : new FailureResponseViewModel<GetCustomerOrdersResponseViewModel>(result.errorCode, result.message);
        }
        #endregion

        #region Cancel Order
        [HttpPut("CancelOrder/{orderNumber:guid}")]
        [Authorization(Role.Customer)]
        public async Task<ResponseViewModel<bool>> CancelOrder(Guid orderNumber)
        {
            string? customerId = Request.GetIdFromToken();
            if (customerId == null)
            {
                return new FailureResponseViewModel<bool>(ErrorCode.UnAuthorized, "Unauthorized");
            }

            var result = await _mediator.Send(new CancelOrderCommand(customerId, orderNumber));
            return result.isSuccess
                ? new SuccessResponseViewModel<bool>(result.data, result.message)
                : new FailureResponseViewModel<bool>(result.errorCode, result.message);
        }

        [HttpGet("CanCancelOrder/{orderNumber:guid}")]
        [Authorization(Role.Customer)]
        public async Task<ResponseViewModel<CanCancelOrderResponseViewModel>> CanCancelOrder(Guid orderNumber)
        {
            string? customerId = Request.GetIdFromToken();
            if (customerId == null)
            {
                return new FailureResponseViewModel<CanCancelOrderResponseViewModel>(ErrorCode.UnAuthorized, "Unauthorized");
            }

            var result = await _mediator.Send(new CanCancelOrderQuery(customerId, orderNumber));
            return result.isSuccess
                ? new SuccessResponseViewModel<CanCancelOrderResponseViewModel>(result.data, result.message)
                : new FailureResponseViewModel<CanCancelOrderResponseViewModel>(result.errorCode, result.message);
        }

        [HttpGet("GetCancellableOrders")]
        [Authorization(Role.Customer)]
        public async Task<ResponseViewModel<IReadOnlyList<CancellableOrderResponseViewModel>>> GetCancellableOrders(
            [FromQuery] PaginationHelper? paginationParams = null)
        {
            string? customerId = Request.GetIdFromToken();
            if (customerId == null)
            {
                return new FailureResponseViewModel<IReadOnlyList<CancellableOrderResponseViewModel>>(ErrorCode.UnAuthorized, "Unauthorized");
            }

            var result = await _mediator.Send(new GetCancellableOrdersQuery(customerId, paginationParams));
            return result.isSuccess
                ? new SuccessResponseViewModel<IReadOnlyList<CancellableOrderResponseViewModel>>(result.data, result.message)
                : new FailureResponseViewModel<IReadOnlyList<CancellableOrderResponseViewModel>>(result.errorCode, result.message);
        }
        #endregion
    }
}
