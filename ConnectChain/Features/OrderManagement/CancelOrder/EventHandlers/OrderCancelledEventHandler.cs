using ConnectChain.Features.NotificationManagement.AddNotificaitonCommand;
using ConnectChain.Features.OrderManagement.CancelOrder.Events;
using MediatR;

namespace ConnectChain.Features.OrderManagement.CancelOrder.EventHandlers
{
    public class OrderCancelledEventHandler(IMediator mediator) : INotificationHandler<OrderCancelledEvent>
    {
        private readonly IMediator _mediator = mediator;

        public async Task Handle(OrderCancelledEvent notification, CancellationToken cancellationToken)
        {
            // Notify all suppliers involved in the cancelled order
            foreach (var supplierId in notification.SupplierIds)
            {
                await _mediator.Send(new AddNotificationCommand(
                    "Order Cancelled by Customer",
                    $"Order #{notification.OrderNumber} has been cancelled by the customer.",
                    "OrderCancellation",
                    supplierId), cancellationToken);
            }
        }
    }
}
