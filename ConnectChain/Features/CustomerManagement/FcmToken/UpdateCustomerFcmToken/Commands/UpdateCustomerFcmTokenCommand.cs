using ConnectChain.Data.Context;
using ConnectChain.Features.CustomerManagement.Common.Queries;
using ConnectChain.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.CustomerManagement.FcmToken.UpdateCustomerFcmToken.Commands
{
    public record UpdateCustomerFcmTokenCommand(string CustomerId, string FcmToken) : IRequest<RequestResult<bool>>;
    
    public class UpdateCustomerFcmTokenCommandHandler(ConnectChainDbContext context, IMediator mediator) 
        : IRequestHandler<UpdateCustomerFcmTokenCommand, RequestResult<bool>>
    {
        private readonly ConnectChainDbContext context = context;
        private readonly IMediator mediator = mediator;

        public async Task<RequestResult<bool>> Handle(UpdateCustomerFcmTokenCommand request, CancellationToken cancellationToken)
        {
            var isCustomerExist = await mediator.Send(new IsCustomerExistsQuery(request.CustomerId), cancellationToken);
            if (!isCustomerExist.isSuccess)
            {
                return RequestResult<bool>.Failure(isCustomerExist.errorCode, isCustomerExist.message);
            }
            
            var rowsAffected = context.Users.Where(c => c.Id == request.CustomerId)
                .ExecuteUpdate(c => c.SetProperty(u => u.FcmToken, request.FcmToken));
                
            if (rowsAffected == 1)
            {
                return RequestResult<bool>.Success(true, "FCM Token updated successfully");
            }
            
            return RequestResult<bool>.Failure(ErrorCode.InternalServerError, "Update failed");
        }
    }
}
