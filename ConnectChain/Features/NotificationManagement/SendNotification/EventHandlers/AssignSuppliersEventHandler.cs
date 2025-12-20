using ConnectChain.Features.NotificationManagement.AddNotificaitonCommand;
using ConnectChain.Features.NotificationManagement.AddNotifications.Command.ConnectChain.Features.NotificationManagement.AddNotificaitonCommand;
using ConnectChain.Features.NotificationManagement.SendNotification.Command;
using ConnectChain.Features.RFQManagement.AssignSuppliersToRFQ.Events;
using ConnectChain.Features.SupplierManagement.FcmToken.GetFcmTokens;
using ConnectChain.Models;
using FluentValidation.Internal;
using MediatR;

namespace ConnectChain.Features.NotificationManagement.SendNotification.EventHandlers
{
    public class AssignSuppliersEventHandler(IMediator mediator) : INotificationHandler<AssignSuppliersEvent>
    {

        private readonly IMediator mediator = mediator;

        public async Task Handle(AssignSuppliersEvent notification, CancellationToken cancellationToken)
        {

            var notificationData = new Notification
            {
                Body = $"تم تعيينك كمورد للطلب رقم {notification.RfqId} , راجع الطلب من قائمة الطلبات",
                Title = $"طلب عرض سعر جدد",
                Type = "RFQ"
            };
            var fcmTokenResult = mediator.Send(new GetFcmTokensQuery(notification.SupplierIds), cancellationToken);

            var fcmToken = fcmTokenResult.Result.data;
            
            if (!(fcmToken == null || !fcmToken.Any()))
            {

                foreach (var token in fcmToken)
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                      var SendResult =  await mediator.Send(new SendNotificationCommand(token, notificationData.Title, notificationData.Body, notificationData.Type), cancellationToken);
                        
                    }
                }
                foreach (var supplierId in notification.SupplierIds)
                {
                    var addResult = await mediator.Send(new AddNotificationCommand(notificationData.Title, notificationData.Body, notificationData.Type, supplierId), cancellationToken);
                }


            }




            //if (!fcmToken.IsNullOrEmpty())
            //{
            //    await mediator.Send(new SendNotificationCommand(fcmToken, notificationData.Title, notificationData.Body, notificationData.Type), cancellationToken);
            //}
            //await mediator.Send(new AddNotificationCommand(notificationData.Title, notificationData.Body, notificationData.Type, notification.Order.SupplierId));

        }
    }
}
