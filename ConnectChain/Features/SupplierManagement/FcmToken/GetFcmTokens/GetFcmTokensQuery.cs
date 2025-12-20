using ConnectChain.Data.Context;
using ConnectChain.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.SupplierManagement.FcmToken.GetFcmTokens
{
    public record GetFcmTokensQuery(List<string> SupplierIds) : IRequest<RequestResult<IReadOnlyList<string>>>;

    public class GetFcmTokensQueryHandler(ConnectChainDbContext context)
        : IRequestHandler<GetFcmTokensQuery, RequestResult<IReadOnlyList<string>>>
    {
        private readonly ConnectChainDbContext context = context;

        public async Task<RequestResult<IReadOnlyList<string>>> Handle(GetFcmTokensQuery request, CancellationToken cancellationToken)
        {
            var tokens = await context.Suppliers
                .Where(s => request.SupplierIds.Contains(s.Id) && s.FcmToken != null)
                .Select(s => s.FcmToken!)
                .ToListAsync(cancellationToken);

            if (!tokens.Any())
                return RequestResult<IReadOnlyList<string>>.Failure(ErrorCode.NotFound, "No FCM tokens found for the given supplier IDs");

            return RequestResult<IReadOnlyList<string>>.Success(tokens);
        }
    }
}
