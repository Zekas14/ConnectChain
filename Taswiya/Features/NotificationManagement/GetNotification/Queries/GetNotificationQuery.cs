using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Notification.GetNotification;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace ConnectChain.Features.NotificationManagement.GetNotification.Queries
{
    public record GetNotificationQuery(string SupplierId) :IRequest<RequestResult<IReadOnlyList<GetNotificationResponseViewModel>>>;
    public class GetNotificationQueryHandler(IRepository<Notification> repository) : IRequestHandler<GetNotificationQuery, RequestResult<IReadOnlyList<GetNotificationResponseViewModel>>>
    {
        private readonly IRepository<Notification> repository = repository;

        public async Task<RequestResult<IReadOnlyList<GetNotificationResponseViewModel>>> Handle(GetNotificationQuery request, CancellationToken cancellationToken)
        {
            var notifications = repository.Get(N=>N.SupplierId ==request.SupplierId).Select(n=> new GetNotificationResponseViewModel
            {
                Id = n.ID, 
                Body = n.Body,
                Type= n.Type,
                IsRead = n.IsRead,
                Date= n.CreatedDate.Date.ToString(),
                Title= n.Title,
            });
            if (notifications.IsNullOrEmpty())
            {
                return RequestResult<IReadOnlyList<GetNotificationResponseViewModel>>.Failure(ErrorCode.NotFound, "No Notification Founded");
            }
            return RequestResult<IReadOnlyList<GetNotificationResponseViewModel>>.Success(notifications.ToList(),"Notification Retreived Successfully");
        }
    }
}
