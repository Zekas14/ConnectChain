using MediatR;
using Microsoft.EntityFrameworkCore;
using ConnectChain.Data.Context;
using ConnectChain.ViewModel.Supplier;
using ConnectChain.Helpers;

namespace ConnectChain.Features.SupplierManagement.Supplier.GetSupplierProfile.Query
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
                .Where(s=>s.Id== request.SupplierId)
                .Select(supplier=>new SupplierProfileViewModel 
                {
                    Name = supplier.Name,
                    PhoneNumber = supplier.PhoneNumber,
                    Address = supplier.Address,
                    Email = supplier.Email,
                 //   ActivityCategory = supplier.ActivityCategory,
                    PaymentMethods = supplier.PaymentMethods.ToList()

                }).FirstOrDefaultAsync(cancellationToken);

            if (supplier == null)
            {
                return RequestResult<SupplierProfileViewModel>.Failure(ErrorCode.NotFound, "Supplier not found.");
            }

            
            return RequestResult<SupplierProfileViewModel>.Success(supplier, "Supplier profile retrieved successfully.");
        }
    }
}
