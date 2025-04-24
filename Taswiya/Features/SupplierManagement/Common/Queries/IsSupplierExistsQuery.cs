using ConnectChain.Data.Context;
using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.SupplierManagement.Common.Queries
{
    public record IsSupplierExistsQuery(string SupplierId) : IRequest<RequestResult<bool>>;
    public class ISupplierExistsQueryHandler(ConnectChainDbContext context) : IRequestHandler<IsSupplierExistsQuery, RequestResult<bool>>
    {
        private readonly ConnectChainDbContext context = context;

        public async Task<RequestResult<bool>> Handle(IsSupplierExistsQuery request, CancellationToken cancellationToken)
        {
            var supplier = await context.Suppliers
                .FirstOrDefaultAsync(s => s.Id == request.SupplierId, cancellationToken);
            if (supplier == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Supplier not found or Not Confirmed");
            }
            return RequestResult<bool>.Success(true, "User exists");
        }
    }

}
