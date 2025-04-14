using MediatR;
using Microsoft.EntityFrameworkCore;
using ConnectChain.Data.Context;
using ConnectChain.ViewModel.Supplier;

namespace ConnectChain.Features.SupplierManagment.GetSupplierProfile.Query
{
    public class GetSupplierProfileQuery : IRequest<SupplierProfileViewModel>
    {
        public string SupplierId { get; set; } = string.Empty;
    }

    public class GetSupplierProfileQueryHandler : IRequestHandler<GetSupplierProfileQuery, SupplierProfileViewModel>
    {
        private readonly ConnectChainDbContext _dbContext;

        public GetSupplierProfileQueryHandler(ConnectChainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SupplierProfileViewModel> Handle(GetSupplierProfileQuery request, CancellationToken cancellationToken)
        {
            var supplier = await _dbContext.Suppliers
                .Include(s => s.ActivityCategory)
                .Include(s => s.PaymentMethods)
                .FirstOrDefaultAsync(s => s.Id == request.SupplierId, cancellationToken);

            if (supplier == null)
            {
                throw new KeyNotFoundException("Supplier not found.");
            }

            return new SupplierProfileViewModel
            {
                FirstName = supplier.FirstName,
                LastName = supplier.LastName,
                PhoneNumber = supplier.PhoneNumber,
                Address = supplier.Address,
                Email = supplier.Email,
                ActivityCategory = supplier.ActivityCategory,
                PaymentMethods = supplier.PaymentMethods.ToList()
            };
        }
    }
}
