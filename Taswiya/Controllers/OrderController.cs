﻿using ConnectChain.Features.OrderManagement.GetOrderDetails.Queries;
using ConnectChain.Features.OrderManagement.GetSupplierOrders.Queries;
using ConnectChain.ViewModel;
using ConnectChain.ViewModel.Order.GetOrderDetails;
using ConnectChain.ViewModel.Order.GetSupplierOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ConnectChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        #region GetSupplierOrders
        [HttpGet("GetSupplierOrders")]
        public async Task<IActionResult> GetSupplierOrders([FromQuery]GetSupplierOrdersRequestViewModel viewModel)
        {
            var result = await _mediator.Send(new GetSupplierOrdersQuery(viewModel.pagination,viewModel.SupplierId));
            if (!result.isSuccess)
            {
                return new FailureResponseViewModel<GetSupplierOrdersResponseViewModel>(result.errorCode, result.message);
            }
            return new SuccessResponseViewModel<IReadOnlyList<GetSupplierOrdersResponseViewModel>>(result.data, result.message);
        }
        #endregion 
        #region GetOrderDetails 
        [HttpGet("GetOrderDetails")]
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
    }
}
