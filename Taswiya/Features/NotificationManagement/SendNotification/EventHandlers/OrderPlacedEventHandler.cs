using ConnectChain.Features.NotificationManagement.AddNotificaitonCommand;
using ConnectChain.Features.NotificationManagement.SendNotification.Command;
using ConnectChain.Features.OrderManagement.PlaceOrder.Events;
using ConnectChain.Features.SupplierManagement.FcmToken.GetFcmToken.Queries;
using ConnectChain.Models;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace ConnectChain.Features.NotificationManagement.SendNotification.EventHandlers
{
    public class OrderPlacedEventHandler(IMediator mediator) : INotificationHandler<OrderPlacedEvent>
    {
        private readonly IMediator mediator = mediator;

        public async Task Handle(OrderPlacedEvent notification, CancellationToken cancellationToken)
        {
            var  fcmTokenResult = await mediator.Send(new GetFcmTokenQuery(notification.Order.SupplierId),cancellationToken);
            var notificationData = CreateNotificationData(notification.Order);
            string fcmToken = fcmTokenResult.data;
            if (!fcmToken.IsNullOrEmpty())
            {
                await mediator.Send(new SendNotificationCommand(fcmToken,notificationData.Title, notificationData.Body, notificationData.Type), cancellationToken);
            }
            await mediator.Send(new AddNotificationCommand(notificationData.Title, notificationData.Body, notificationData.Type,notification.Order.SupplierId));
        }
        private static Notification CreateNotificationData(Order order)
        {
            return new Notification
            {
                Body = $"تم اضافة طلب جديد بتاريخ {order.CreatedDate.Date.ToString()} , راجع الطلب من قائمة الطلبات",
                Title = $"طلب جديد",
                Type = "Order"
            };
        }
    }
}
