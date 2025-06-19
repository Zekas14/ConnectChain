using ConnectChain.Data.Context;
using ConnectChain.Helpers;
using ConnectChain.ViewModel.Supplier.FindSuppliers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ConnectChain.Features.SupplierManagement.FindSuppliers.Queries
{
    public record FilterSuppliersByRatingQuery(
        double MinRating = 0.0, 
        double MaxRating = 5.0, 
        string? BusinessType = null) 
        : IRequest<RequestResult<IReadOnlyList<FindSuppliersResponseViewModel>>>;
    
    public class FilterSuppliersByRatingQueryHandler(ConnectChainDbContext context) 
        : IRequestHandler<FilterSuppliersByRatingQuery, RequestResult<IReadOnlyList<FindSuppliersResponseViewModel>>>
    {
        private readonly ConnectChainDbContext _context = context;

        public async Task<RequestResult<IReadOnlyList<FindSuppliersResponseViewModel>>> Handle(FilterSuppliersByRatingQuery request, CancellationToken cancellationToken)
        {
            if (request.MinRating < 0 || request.MaxRating > 5 || request.MinRating > request.MaxRating)
            {
                return RequestResult<IReadOnlyList<FindSuppliersResponseViewModel>>.Failure(ErrorCode.BadRequest, "Invalid rating range. Rating should be between 0 and 5, and MinRating should be less than or equal to MaxRating.");
            }

            var query = _context.Suppliers
                .Include(s => s.ActivityCategory)
                .Include(s => s.Rate)
                .Include(s => s.PaymentMethods)
                .Include(s => s.Products)
                .Where(s => s.Rate.Any()) 
                .AsEnumerable() 
                .Where(s => 
                {
                    var avgRating = s.Rate.Average(r => r.RateNumber);
                    return avgRating >= request.MinRating && avgRating <= request.MaxRating;
                })
                .Where(s => string.IsNullOrEmpty(request.BusinessType) || 
                           (s.BusinessType != null && s.BusinessType.Equals(request.BusinessType, StringComparison.OrdinalIgnoreCase)))
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

         
            var suppliers = query.ToList();

            if (suppliers.IsNullOrEmpty())
            {
                return RequestResult<IReadOnlyList<FindSuppliersResponseViewModel>>.Failure(ErrorCode.NotFound, $"No suppliers found with rating between {request.MinRating} and {request.MaxRating}.");
            }

            return RequestResult<IReadOnlyList<FindSuppliersResponseViewModel>>.Success(suppliers, "Suppliers filtered successfully.");
        }
    }
}
