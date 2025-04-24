using ConnectChain.Features.NotificationManagement.AddNotificaitonCommand;
using ConnectChain.Features.NotificationManagement.SendNotification.Command;
using ConnectChain.Features.OrderManagement.PlaceOrder.Events;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace ConnectChain.Features.NotificationManagement.EventHandlers
{
    public class OrderPlacedEventHandler(IMediator mediator) : INotificationHandler<OrderPlacedEvent>
    {
        private readonly IMediator mediator = mediator;

        public async Task Handle(OrderPlacedEvent notification, CancellationToken cancellationToken)
        {
            if (!notification.FcmToken.IsNullOrEmpty())
            {
                await mediator.Send(new SendNotificationCommand(notification.FcmToken, "اوردر جديد", "تم طلب اوردر جديد", "Order"));
                await mediator.Send(new AddNotificationCommand("اوردر جديد", "تم طلب اوردر جديد", "Order"));
            }
        }
    }
}
