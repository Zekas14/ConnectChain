using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ConnectChain.Features.SupplierManagement.Common.Queries
{
    public record IsUserExistsQuery(string UserId) : IRequest<RequestResult<bool>>;
    public class IsUserExistsQueryHandler(UserManager<User> userManager) : IRequestHandler<IsUserExistsQuery, RequestResult<bool>>
    {
        private readonly UserManager<User> userManager = userManager;

        public async Task<RequestResult<bool>> Handle(IsUserExistsQuery request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "User not found");
            }
            return RequestResult<bool>.Success(true, "User exists");
        }
    }

}
