using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.OrderManagement.CancelOrder.Queries
{
    public record CanCancelOrderQuery(string CustomerId, Guid OrderNumber) : IRequest<RequestResult<CanCancelOrderResponseViewModel>>;
    
    public class CanCancelOrderResponseViewModel
    {
        public bool CanCancel { get; set; }
        public string Reason { get; set; } = string.Empty;
        public double HoursRemaining { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
    }
    
    public class CanCancelOrderQueryHandler(IRepository<Order> repository) 
        : IRequestHandler<CanCancelOrderQuery, RequestResult<CanCancelOrderResponseViewModel>>
    {
        private readonly IRepository<Order> _repository = repository;

        public async Task<RequestResult<CanCancelOrderResponseViewModel>> Handle(CanCancelOrderQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.CustomerId))
            {
                return RequestResult<CanCancelOrderResponseViewModel>.Failure(ErrorCode.BadRequest, "Customer ID cannot be empty.");
            }

            // Get the first order with the order number for this customer
            var order = await _repository.Get(o => o.CustomerId == request.CustomerId && 
                                                  o.OrderNumber == request.OrderNumber)
                .FirstOrDefaultAsync(cancellationToken);

            if (order == null)
            {
                return RequestResult<CanCancelOrderResponseViewModel>.Failure(ErrorCode.NotFound, "Order not found.");
            }

            var response = new CanCancelOrderResponseViewModel
            {
                OrderStatus = order.Status.ToString(),
                OrderDate = order.CreatedDate
            };

            // Check if order status allows cancellation
            if (order.Status == OrderStatus.Canceled)
            {
                response.CanCancel = false;
                response.Reason = "Order is already cancelled.";
                response.HoursRemaining = 0;
                return RequestResult<CanCancelOrderResponseViewModel>.Success(response, "Order cancellation status retrieved.");
            }

            if (order.Status == OrderStatus.Completed)
            {
                response.CanCancel = false;
                response.Reason = "Cannot cancel a completed order.";
                response.HoursRemaining = 0;
                return RequestResult<CanCancelOrderResponseViewModel>.Success(response, "Order cancellation status retrieved.");
            }

            if (order.Status == OrderStatus.Rejected)
            {
                response.CanCancel = false;
                response.Reason = "Cannot cancel a rejected order.";
                response.HoursRemaining = 0;
                return RequestResult<CanCancelOrderResponseViewModel>.Success(response, "Order cancellation status retrieved.");
            }

            // Check if order is within 1 day (24 hours) of creation
            var timeSinceCreation = DateTime.Now - order.CreatedDate;
            var hoursRemaining = 24 - timeSinceCreation.TotalHours;

            if (timeSinceCreation.TotalDays > 1)
            {
                response.CanCancel = false;
                response.Reason = "Cannot cancel order after 24 hours of placement.";
                response.HoursRemaining = 0;
            }
            else
            {
                response.CanCancel = true;
                response.Reason = "Order can be cancelled.";
                response.HoursRemaining = Math.Max(0, hoursRemaining);
            }

            return RequestResult<CanCancelOrderResponseViewModel>.Success(response, "Order cancellation status retrieved.");
        }
    }
}
