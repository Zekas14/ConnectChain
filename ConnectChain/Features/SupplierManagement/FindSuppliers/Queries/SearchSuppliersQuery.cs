using ConnectChain.Data.Context;
using ConnectChain.Helpers;
using ConnectChain.ViewModel.Supplier.FindSuppliers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ConnectChain.Features.SupplierManagement.FindSuppliers.Queries
{
    public record SearchSuppliersQuery(
        string? SearchName = null,
        string? BusinessType = null,
        double? MinRating = null,
        double? MaxRating = null)
        : IRequest<RequestResult<IReadOnlyList<FindSuppliersResponseViewModel>>>;
    
    public class SearchSuppliersQueryHandler(ConnectChainDbContext context) 
        : IRequestHandler<SearchSuppliersQuery, RequestResult<IReadOnlyList<FindSuppliersResponseViewModel>>>
    {
        private readonly ConnectChainDbContext _context = context;

        public async Task<RequestResult<IReadOnlyList<FindSuppliersResponseViewModel>>> Handle(SearchSuppliersQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Suppliers
                .Include(s => s.Rate)
                .Include(s => s.PaymentMethods)
                .Include(s => s.Products)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchName))
            {
                query = query.Where(s => s.Name != null && s.Name.Contains(request.SearchName));
            }

            if (!string.IsNullOrWhiteSpace(request.BusinessType))
            {
                query = query.Where(s => s.BusinessType != null && s.BusinessType.Equals(request.BusinessType, StringComparison.OrdinalIgnoreCase));
            }

            var suppliersData = await query.ToListAsync(cancellationToken);

            var filteredSuppliers = suppliersData.AsEnumerable();

            if (request.MinRating.HasValue || request.MaxRating.HasValue)
            {
                filteredSuppliers = filteredSuppliers.Where(s => s.Rate.Any()); // Only suppliers with ratings

                if (request.MinRating.HasValue)
                {
                    filteredSuppliers = filteredSuppliers.Where(s => s.Rate.Average(r => r.RateNumber) >= request.MinRating.Value);
                }

                if (request.MaxRating.HasValue)
                {
                    filteredSuppliers = filteredSuppliers.Where(s => s.Rate.Average(r => r.RateNumber) <= request.MaxRating.Value);
                }
            }

            var suppliers = filteredSuppliers
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

             var result = suppliers.ToList();

            if (result.IsNullOrEmpty())
            {
                return RequestResult<IReadOnlyList<FindSuppliersResponseViewModel>>.Failure(ErrorCode.NotFound, "No suppliers found matching the specified criteria.");
            }

            return RequestResult<IReadOnlyList<FindSuppliersResponseViewModel>>.Success(result, "Suppliers found successfully.");
        }
    }
}
