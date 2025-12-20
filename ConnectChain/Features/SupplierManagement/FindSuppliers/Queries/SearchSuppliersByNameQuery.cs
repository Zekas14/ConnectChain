using ConnectChain.Data.Context;
using ConnectChain.Helpers;
using ConnectChain.ViewModel.Supplier.FindSuppliers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ConnectChain.Features.SupplierManagement.FindSuppliers.Queries
{
    public record SearchSuppliersByNameQuery(string SearchName) 
        : IRequest<RequestResult<IReadOnlyList<FindSuppliersResponseViewModel>>>;
    
    public class SearchSuppliersByNameQueryHandler(ConnectChainDbContext context) 
        : IRequestHandler<SearchSuppliersByNameQuery, RequestResult<IReadOnlyList<FindSuppliersResponseViewModel>>>
    {
        private readonly ConnectChainDbContext _context = context;

        public async Task<RequestResult<IReadOnlyList<FindSuppliersResponseViewModel>>> Handle(SearchSuppliersByNameQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.SearchName))
            {
                return RequestResult<IReadOnlyList<FindSuppliersResponseViewModel>>.Failure(ErrorCode.BadRequest, "Search name cannot be empty.");
            }

            var query = _context.Suppliers
                .Include(s => s.ActivityCategory)
                .Include(s => s.Rate)
                .Include(s => s.PaymentMethods)
                .Include(s => s.Products)
                .Where(s => s.Name != null && s.Name.Contains(request.SearchName))
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
                return RequestResult<IReadOnlyList<FindSuppliersResponseViewModel>>.Failure(ErrorCode.NotFound, $"No suppliers found with name containing '{request.SearchName}'.");
            }

            return RequestResult<IReadOnlyList<FindSuppliersResponseViewModel>>.Success(suppliers, "Suppliers found successfully.");
        }
    }
}
