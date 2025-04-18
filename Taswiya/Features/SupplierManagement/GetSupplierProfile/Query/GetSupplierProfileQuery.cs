using MediatR;
using Microsoft.EntityFrameworkCore;
using ConnectChain.Data.Context;
using ConnectChain.ViewModel.Supplier;
using ConnectChain.Helpers;

namespace ConnectChain.Features.SupplierManagement.GetSupplierProfile.Query
{
    public class GetSupplierProfileQuery : IRequest<RequestResult<SupplierProfileViewModel>>
    {
        public string SupplierId { get; set; } = string.Empty;
    }

    public class GetSupplierProfileQueryHandler : IRequestHandler<GetSupplierProfileQuery, RequestResult<SupplierProfileViewModel>>
    {
        private readonly ConnectChainDbContext _dbContext;

        public GetSupplierProfileQueryHandler(ConnectChainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RequestResult<SupplierProfileViewModel>> Handle(GetSupplierProfileQuery request, CancellationToken cancellationToken)
        {
            var supplier = await _dbContext.Suppliers
                .Include(s => s.ActivityCategory)
                .Include(s => s.PaymentMethods)

                .FirstOrDefaultAsync(s => s.Id == request.SupplierId, cancellationToken);

            if (supplier == null)
            {
                return RequestResult<SupplierProfileViewModel>.Failure(ErrorCode.NotFound, "Supplier not found.");
            }

            SupplierProfileViewModel data = new SupplierProfileViewModel
            {

                PhoneNumber = supplier.PhoneNumber,
                Address = supplier.Address,
                Email = supplier.Email,
                ActivityCategory = supplier.ActivityCategory,
                PaymentMethods = supplier.PaymentMethods.ToList()
            };
            return RequestResult<SupplierProfileViewModel>.Success(data, "Supplier profile retrieved successfully.");
        }
    }
}
