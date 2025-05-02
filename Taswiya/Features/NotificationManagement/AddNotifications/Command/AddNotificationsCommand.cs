using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;

namespace ConnectChain.Features.NotificationManagement.AddNotifications.Command
{
    namespace ConnectChain.Features.NotificationManagement.AddNotificaitonCommand
    {
        public record AddNotificationsCommand(List<NotificationCreateModel> Notifications) : IRequest<RequestResult<bool>>;

        public record NotificationCreateModel(string? Title, string? Body, string? Type, string SupplierId);

        public class AddNotificationsHandler(IRepository<Notification> repository)
            : IRequestHandler<AddNotificationsCommand, RequestResult<bool>>
        {
            private readonly IRepository<Notification> repository = repository;

            public async Task<RequestResult<bool>> Handle(AddNotificationsCommand request, CancellationToken cancellationToken)
            {
                var notifications = request.Notifications.Select(n => new Notification
                {
                    Title = n.Title,
                    Body = n.Body,
                    SupplierId = n.SupplierId,
                    Type = n.Type,
                    IsRead = false,
                }).ToList();

                repository.AddRange(notifications);
                await repository.SaveChangesAsync();

                return RequestResult<bool>.Success(true, "Notifications Added");
            }
        }
    }
}
