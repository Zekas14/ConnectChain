using ConnectChain.Data.Context;
using ConnectChain.Features.SupplierManagement.Common.Queries;
using ConnectChain.Features.SupplierManagement.FcmToken.GetFcmToken.Queries;
using ConnectChain.Helpers;
using MediatR;

namespace ConnectChain.Features.SupplierManagement.FcmToken.GetFcmToken.Queries
{
    public record GetFcmTokenQuery(string SupplierId): IRequest<RequestResult<string>>;
    public class GetFcmTokenQueryHandler(ConnectChainDbContext context, IMediator mediator) : IRequestHandler<GetFcmTokenQuery, RequestResult<string>>
    {
        private readonly ConnectChainDbContext context = context;
        private readonly IMediator mediator = mediator;

        public async Task<RequestResult<string>> Handle(GetFcmTokenQuery request, CancellationToken cancellationToken)
        {
            var isSupplierExistsResult = await mediator.Send(new IsSupplierExistsQuery(request.SupplierId));
            if (!isSupplierExistsResult.isSuccess)
            {
                return RequestResult<string>.Failure(isSupplierExistsResult.errorCode, isSupplierExistsResult.message);
            }
            var data = context.Suppliers.FirstOrDefault(s =>s.Id==request.SupplierId)!.FcmToken;
            return RequestResult<string>.Success(data);
        }
    }
}
