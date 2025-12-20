using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.NotificationManagement.AddCustomerNotification.Commands;
using ConnectChain.Features.NotificationManagement.SendNotification.Command;
using ConnectChain.Features.CustomerManagement.FcmToken.GetCustomerFcmToken.Queries;
using ConnectChain.Features.OrderManagement.CancelOrder.Events;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ConnectChain.Features.OrderManagement.CancelOrder.Commands
{
    public record CancelOrderCommand(string CustomerId, Guid OrderNumber) : IRequest<RequestResult<bool>>;
    
    public class CancelOrderCommandHandler(IRepository<Order> repository, IMediator mediator) 
        : IRequestHandler<CancelOrderCommand, RequestResult<bool>>
    {
        private readonly IRepository<Order> _repository = repository;
        private readonly IMediator _mediator = mediator;

        public async Task<RequestResult<bool>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.CustomerId))
            {
                return RequestResult<bool>.Failure(ErrorCode.BadRequest, "Customer ID cannot be empty.");
            }

            // Get all orders with the same order number for this customer
            var orders = await _repository.Get(o => o.CustomerId == request.CustomerId && 
                                                   o.OrderNumber == request.OrderNumber)
                .Include(o => o.Supplier)
                .ToListAsync(cancellationToken);

            if (!orders.Any())
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Order not found.");
            }

            var firstOrder = orders.First();

            // Check if order can be cancelled (within 1 day and status allows cancellation)
            var canCancel = CanCancelOrder(firstOrder);
            if (!canCancel.isSuccess)
            {
                return RequestResult<bool>.Failure(canCancel.errorCode, canCancel.message);
            }

            // Update all orders with the same order number to Canceled status
            foreach (var order in orders)
            {
                order.Status = OrderStatus.Canceled;
                order.UpdatedDate = DateTime.Now;
                _repository.Update(order);
            }

            await _repository.SaveChangesAsync();

            // Send notification to customer about order cancellation
            var notificationTitle = "Order Cancelled";
            var notificationBody = $"Your order #{request.OrderNumber} has been successfully cancelled.";
            var notificationType = "OrderCancellation";

            await _mediator.Send(new AddCustomerNotificationCommand(
                notificationTitle,
                notificationBody,
                notificationType,
                request.CustomerId), cancellationToken);

            // Send push notification to customer if FCM token is available
            var fcmTokenResult = await _mediator.Send(new GetCustomerFcmTokenQuery(request.CustomerId), cancellationToken);
            if (fcmTokenResult.isSuccess && !fcmTokenResult.data.IsNullOrEmpty())
            {
                await _mediator.Send(new SendNotificationCommand(
                    fcmTokenResult.data,
                    notificationTitle,
                    notificationBody,
                    notificationType), cancellationToken);
            }

            // Publish event to notify suppliers
            var supplierIds = orders.Select(o => o.SupplierId).Distinct().ToList();
            await _mediator.Publish(new OrderCancelledEvent(request.OrderNumber, request.CustomerId, supplierIds), cancellationToken);

            return RequestResult<bool>.Success(true, "Order cancelled successfully.");
        }

        private RequestResult<bool> CanCancelOrder(Order order)
        {
            if (order.Status == OrderStatus.Canceled)
            {
                return RequestResult<bool>.Failure(ErrorCode.BadRequest, "Order is already cancelled.");
            }

            if (order.Status == OrderStatus.Completed)
            {
                return RequestResult<bool>.Failure(ErrorCode.BadRequest, "Cannot cancel a completed order.");
            }

            if (order.Status == OrderStatus.Rejected)
            {
                return RequestResult<bool>.Failure(ErrorCode.BadRequest, "Cannot cancel a rejected order.");
            }

            var timeSinceCreation = DateTime.Now - order.CreatedDate;
            if (timeSinceCreation.TotalDays > 1)
            {
                return RequestResult<bool>.Failure(ErrorCode.BadRequest, "Cannot cancel order after 24 hours of placement.");
            }

            return RequestResult<bool>.Success(true, "Order can be cancelled.");
        }
    }
}
