using ConnectChain.Features.OrderManagement.PlaceOrder.Command;
using ConnectChain.Features.QuotationManagement.ApproveQuotation.Event;
using MediatR;

namespace ConnectChain.Features.OrderManagement.PlaceOrder.EventHandlers
{
    public class ApproveQuotationEventHandler(IMediator mediator) : INotificationHandler<ApproveQuotationEvent>
    {
        private readonly IMediator _mediator = mediator;

        public async Task Handle(ApproveQuotationEvent notification, CancellationToken cancellationToken)
        {
           await _mediator.Send(new  PlaceOrderFromQuotationCommand(notification.QuotationId),cancellationToken);
        }
    }
}
