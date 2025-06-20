using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.WishlistManagement.GetWishlistProducts.Queries;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Product.GetCustomerProducts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ConnectChain.Features.ProductManagement.GetCustomerProducts.Queries
{
    public record GetProductsByBusinessTypeQuery(string BusinessType, string? CustomerId = null, PaginationHelper? PaginationParams = null) 
        : IRequest<RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>>;
    
    public class GetProductsByBusinessTypeQueryHandler(IRepository<Product> repository, IMediator mediator) 
        : IRequestHandler<GetProductsByBusinessTypeQuery, RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>>
    {
        private readonly IRepository<Product> _repository = repository;
        private readonly IMediator _mediator = mediator;

        public async Task<RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>> Handle(GetProductsByBusinessTypeQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.BusinessType))
            {
                return RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>.Failure(ErrorCode.BadRequest, "Business type cannot be empty.");
            }

            var customerWishlistProductIds = new List<int>();
            if (!string.IsNullOrWhiteSpace(request.CustomerId))
            {
                var wishlistResult = await _mediator.Send(new GetWishlistProductsQuery(request.CustomerId), cancellationToken);
                customerWishlistProductIds = wishlistResult.isSuccess 
                    ? wishlistResult.data.Select(w => w.ProductId).ToList() 
                    : new List<int>();
            }

            var query = _repository.Get(p => !p.Deleted && 
                                            p.Supplier != null && 
                                            p.Supplier.BusinessType != null &&
                                            p.Supplier.BusinessType.Equals(request.BusinessType, StringComparison.OrdinalIgnoreCase))
                .Include(p => p.Supplier)
                    .ThenInclude(s => s.Rate)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Select(p => new GetCustomerProductsResponseViewModel
                {
                    ProductId = p.ID,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    CategoryName = p.Category != null ? p.Category.Name : null,
                    Image = p.Images.Where(i => !i.Deleted).Select(i => i.Url).FirstOrDefault() ?? string.Empty,
                    IsStockAvailable = p.MinimumStock < p.Stock,
                    SupplierName = p.Supplier.Name ?? string.Empty,
                    SupplierId = p.Supplier.Id,
                    BusinessType = p.Supplier.BusinessType ?? string.Empty,
                    SupplierRating = p.Supplier.Rate.Any() ? p.Supplier.Rate.Average(r => r.RateNumber) : 0.0,
                    CreatedDate = p.CreatedDate,
                    IsInWishlist = customerWishlistProductIds.Contains(p.ID)
                })
                .OrderByDescending(p => p.SupplierRating)
                .ThenByDescending(p => p.CreatedDate);

            

            var products = await query.ToListAsync(cancellationToken);

            if (products.IsNullOrEmpty())
            {
                return RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>.Failure(ErrorCode.NotFound, $"No products found from suppliers with business type '{request.BusinessType}'.");
            }

            return RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>.Success(products, "Products retrieved successfully.");
        }
    }
}
