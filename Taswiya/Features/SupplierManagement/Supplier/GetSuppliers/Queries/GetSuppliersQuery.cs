using ConnectChain.Data.Context;
using ConnectChain.Helpers;
using ConnectChain.MLModels;
using ConnectChain.Models;
using ConnectChain.ViewModel.Supplier.GetSuppliers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ConnectChain.Features.SupplierManagement.Supplier.GetSuppliers.Queries
{
    public record GetSuppliersQuery(string CustomerId) : IRequest<RequestResult<IReadOnlyList<GetSuppliersResponseViewModel>>>;
    public class GetSuppliersQueryHandler(ConnectChainDbContext context) : 
        IRequestHandler<GetSuppliersQuery, RequestResult<IReadOnlyList<GetSuppliersResponseViewModel>>>
    {
        private readonly ConnectChainDbContext context = context;

        public async Task<RequestResult<IReadOnlyList<GetSuppliersResponseViewModel>>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
        {
            var customer = await context.Set<Customer>()
        .FirstOrDefaultAsync(c => c.Id == request.CustomerId, cancellationToken);

            if (customer is null)
            {
                return RequestResult<IReadOnlyList<GetSuppliersResponseViewModel>>.Failure(ErrorCode.NotFound,"Customer Not Found");
            }

            var suppliers =  context.Suppliers
                .Include(s => s.Products)
                .ThenInclude(p => p.Category)
                .Include(s => s.Rate)
                .Select(s=>new
                {
                    s.Id,
                    s.Name,
                    s.ImageUrl,
                    s.Address,
                     s.BusinessType,
                    Rating = s.Rate.Any() ? (float)s.Rate.Average(r => r.RateNumber) : 0f
                }).ToList();

            var matcher = new SupplierMatcher("MLModels/MatchingModel.zip");

            var matchedSuppliers =  suppliers
                .Where(s => matcher.IsMatch(new SupplierInput
                {
                    BusinessType = s.BusinessType ?? "unKnown",
                    Address = s.Address ?? "unKnown",
                    Rating = s.Rating,
                    Category = s.BusinessType
                })).ToList();

            var result = matchedSuppliers.Select(s => new GetSuppliersResponseViewModel
            {
                Id = s.Id,
                Name = s.Name,
                BusinessType = s.BusinessType,
                Rating = s.Rating,
            }).ToList();
            if (result.IsNullOrEmpty())
            {
                return RequestResult<IReadOnlyList<GetSuppliersResponseViewModel>>.Failure(ErrorCode.NotFound, "No Suppliers Matched");
            }
            return RequestResult<IReadOnlyList<GetSuppliersResponseViewModel>>.Success(result,"Success");
        }
    }
}
