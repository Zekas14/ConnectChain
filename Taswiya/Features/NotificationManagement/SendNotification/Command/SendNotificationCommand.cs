using ConnectChain.Helpers;
using FirebaseAdmin.Messaging;
using MediatR;

namespace ConnectChain.Features.NotificationManagement.SendNotification.Command
{
    public record SendNotificationCommand(string DeviceToken , string Title , string Body , string Type):IRequest<RequestResult<bool>>;
    public class SendNotificationCommandHandler () : IRequestHandler<SendNotificationCommand, RequestResult<bool>>
    {
        public async Task<RequestResult<bool>> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
        {
            var message = new Message()
            {
                Token =request.DeviceToken,
                Notification = new Notification
                {
                    Title = request.Title,
                    Body = request.Body
                },
                
                
            };
           
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message,cancellationToken);
            Console.WriteLine("Successfully sent message: " + response);
            return RequestResult<bool>.Success(true);
            
        }
    }
}

