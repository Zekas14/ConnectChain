using ConnectChain.Data.Context;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Supplier.FindSuppliers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ConnectChain.Features.SupplierManagement.FindSuppliers.Queries
{
    public record GetSuppliersByBusinessTypeQuery(string CustomerId) 
        : IRequest<RequestResult<IReadOnlyList<FindSuppliersResponseViewModel>>>;
    
    public class GetSuppliersByBusinessTypeQueryHandler(ConnectChainDbContext context) 
        : IRequestHandler<GetSuppliersByBusinessTypeQuery, RequestResult<IReadOnlyList<FindSuppliersResponseViewModel>>>
    {
        private readonly ConnectChainDbContext _context = context;

        public async Task<RequestResult<IReadOnlyList<FindSuppliersResponseViewModel>>> Handle(GetSuppliersByBusinessTypeQuery request, CancellationToken cancellationToken)
        {

            var customerBusinessType = _context.Set<Customer>().Where(c => c.Id == request.CustomerId).Select(c => c.BusinessType).FirstOrDefault();

            var query = _context.Suppliers
                .Include(s => s.ActivityCategory)
                .Include(s => s.Rate)
                .Include(s => s.PaymentMethods)
                .Include(s => s.Products)
                .Where(s => s.BusinessType != null && s.BusinessType.Equals(customerBusinessType, StringComparison.OrdinalIgnoreCase))
                .Select(s => new FindSuppliersResponseViewModel
                {
                    Id = s.Id,
                    Name = s.Name ?? string.Empty,
                    Email = s.Email ?? string.Empty,
                    PhoneNumber = s.PhoneNumber ?? string.Empty,
                    Address = s.Address ?? string.Empty,
                    BusinessType = s.BusinessType ?? string.Empty,
                    ImageUrl = s.ImageUrl ?? string.Empty,
                    ActivityCategoryName = s.ActivityCategory != null ? s.ActivityCategory.Name : string.Empty,
                    AverageRating = s.Rate.Any() ? s.Rate.Average(r => r.RateNumber) : 0.0,
                    TotalRatings = s.Rate.Count(),
                    TotalProducts = s.Products.Count(p => !p.Deleted),
                    PaymentMethods = s.PaymentMethods.Select(pm => pm.Name).ToList()
                })
                .OrderByDescending(s => s.AverageRating)
                .ThenBy(s => s.Name);

         
            var suppliers = await query.ToListAsync(cancellationToken);

            if (suppliers.IsNullOrEmpty())
            {
                return RequestResult<IReadOnlyList<FindSuppliersResponseViewModel>>.Failure(ErrorCode.NotFound, $"No suppliers found with business type '{customerBusinessType}'.");
            }

            return RequestResult<IReadOnlyList<FindSuppliersResponseViewModel>>.Success(suppliers, "Suppliers retrieved successfully.");
        }
    }
}
