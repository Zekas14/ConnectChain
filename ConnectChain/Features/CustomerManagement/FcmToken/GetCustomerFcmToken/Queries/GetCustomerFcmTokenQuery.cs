using ConnectChain.Data.Context;
using ConnectChain.Features.CustomerManagement.Common.Queries;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;

namespace ConnectChain.Features.CustomerManagement.FcmToken.GetCustomerFcmToken.Queries
{
    public record GetCustomerFcmTokenQuery(string CustomerId) : IRequest<RequestResult<string>>;
    
    public class GetCustomerFcmTokenQueryHandler(ConnectChainDbContext context, IMediator mediator) 
        : IRequestHandler<GetCustomerFcmTokenQuery, RequestResult<string>>
    {
        private readonly ConnectChainDbContext context = context;
        private readonly IMediator mediator = mediator;

        public async Task<RequestResult<string>> Handle(GetCustomerFcmTokenQuery request, CancellationToken cancellationToken)
        {
            var isCustomerExistsResult = await mediator.Send(new IsCustomerExistsQuery(request.CustomerId), cancellationToken);
            if (!isCustomerExistsResult.isSuccess)
            {
                return RequestResult<string>.Failure(isCustomerExistsResult.errorCode, isCustomerExistsResult.message);
            }
            
            var fcmToken = context.Set<Customer>().FirstOrDefault(c => c.Id == request.CustomerId)?.FcmToken;
            return RequestResult<string>.Success(fcmToken ?? string.Empty);
        }
    }
}
