using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;

namespace ConnectChain.Features.NotificationManagement.AddCustomerNotification.Commands
{
    public record AddCustomerNotificationCommand(string? Title, string? Body, string? Type, string CustomerId) 
        : IRequest<RequestResult<bool>>;
    
    public class AddCustomerNotificationCommandHandler(IRepository<Notification> repository) 
        : IRequestHandler<AddCustomerNotificationCommand, RequestResult<bool>>
    {
        private readonly IRepository<Notification> _repository = repository;

        public async Task<RequestResult<bool>> Handle(AddCustomerNotificationCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.CustomerId))
            {
                return RequestResult<bool>.Failure(ErrorCode.BadRequest, "Customer ID cannot be empty.");
            }

            var notification = new Notification()
            {
                Title = request.Title,
                Body = request.Body,
                UserId= request.CustomerId,
                Type = request.Type,
                IsRead = false,
            };

            await _repository.AddAsync(notification);
            await _repository.SaveChangesAsync();
            
            return RequestResult<bool>.Success(true, "Customer notification added successfully.");
        }
    }
}
