    using ConnectChain.Helpers;
    using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2.Responses;
using MediatR;

    namespace ConnectChain.Features.NotificationManagement.SendNotification.Command
    {
        public record SendNotificationCommand(string DeviceToken , string Title , string Body , string Type):IRequest<RequestResult<bool>>;
        public class SendNotificationCommandHandler () : IRequestHandler<SendNotificationCommand, RequestResult<bool>>
        {
            public async Task<RequestResult<bool>> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
            {
            try
            {
                var message = new Message()
                {
                    Token = request.DeviceToken,
                    Notification = new Notification
                    {
                        Title = request.Title,
                        Body = request.Body
                    },

                };
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message, cancellationToken);
                Console.WriteLine("Successfully sent message: " + response);
                return RequestResult<bool>.Success(true, "Successfully sent message");

            }
            catch (FirebaseMessagingException ex) when (ex.MessagingErrorCode == MessagingErrorCode.Unregistered)
            {
                return RequestResult<bool>.Failure(ErrorCode.InvalidInput,"Invalid Token. Please refresh.");

            } 
            catch (TokenResponseException ex) 
            {
                return RequestResult<bool>.Failure(ErrorCode.InvalidInput,$"{ex.Message}");

            } 
            }
        }
    }

