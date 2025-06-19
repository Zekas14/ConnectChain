using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.CustomerManagement.Common.Queries;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.CustomerManagement.UpdateFcmToken.Commands
{
    public record UpdateCustomerFcmTokenCommand(string CustomerId, string FcmToken) : IRequest<RequestResult<bool>>;
    
    public class UpdateCustomerFcmTokenCommandHandler(IRepository<Customer> repository, IMediator mediator) 
        : IRequestHandler<UpdateCustomerFcmTokenCommand, RequestResult<bool>>
    {
        private readonly IRepository<Customer> _repository = repository;
        private readonly IMediator _mediator = mediator;

        public async Task<RequestResult<bool>> Handle(UpdateCustomerFcmTokenCommand request, CancellationToken cancellationToken)
        {
            var isCustomerExist = await _mediator.Send(new IsCustomerExistsQuery(request.CustomerId), cancellationToken);
            if (!isCustomerExist.isSuccess)
            {
                return RequestResult<bool>.Failure(isCustomerExist.errorCode, isCustomerExist.message);
            }

            var rowsAffected = await _repository.Get(c => c.Id == request.CustomerId)
                .ExecuteUpdateAsync(c => c.SetProperty(customer => customer.FcmToken, request.FcmToken), cancellationToken);

            if (rowsAffected == 1)
            {
                return RequestResult<bool>.Success(true, "FCM Token updated successfully");
            }

            return RequestResult<bool>.Failure(ErrorCode.InternalServerError, "Update failed");
        }
    }
}
