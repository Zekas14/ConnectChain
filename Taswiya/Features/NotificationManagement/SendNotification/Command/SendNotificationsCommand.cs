    using ConnectChain.Helpers;
    using FirebaseAdmin.Messaging;
    using MediatR;

    namespace ConnectChain.Features.NotificationManagement.SendNotification.Command
    {
        public record SendNotificationsCommand(List<string> DeviceToken , string Title , string Body , string Type):IRequest<RequestResult<bool>>;
        public class SendNotificationsCommandHandler () : IRequestHandler<SendNotificationsCommand, RequestResult<bool>>
        {
            public async Task<RequestResult<bool>> Handle(SendNotificationsCommand request, CancellationToken cancellationToken)
            {
            try
            {
                var message = new MulticastMessage()
                {
                    Tokens = request.DeviceToken,
                    Notification = new Notification
                    {
                        Title = request.Title,
                        Body = request.Body
                    },

                };
                var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message, cancellationToken);
                Console.WriteLine("Successfully sent message: " + response.SuccessCount);
                return RequestResult<bool>.Success(true, "Successfully sent message");

            }
            catch (FirebaseMessagingException ex) when (ex.MessagingErrorCode == MessagingErrorCode.Unregistered)
            {
                return RequestResult<bool>.Failure(ErrorCode.InvalidInput,"Invalid Token. Please refresh.");

            } 
            }
        }
    }

