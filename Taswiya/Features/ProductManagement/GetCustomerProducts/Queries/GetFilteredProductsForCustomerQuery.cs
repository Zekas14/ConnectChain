using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.CustomerManagement.GetCustomerById.Queries;
using ConnectChain.Features.WishlistManagement.GetWishlistProducts.Queries;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Product.GetCustomerProducts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ConnectChain.Features.ProductManagement.GetCustomerProducts.Queries
{
    public record GetFilteredProductsForCustomerQuery(
        string? CustomerId = null,
        string? BusinessType = null,
        bool MatchCustomerBusinessType = false,
        int? CategoryId = null,
        decimal? MinPrice = null,
        decimal? MaxPrice = null,
        double? MinSupplierRating = null,
        bool OnlyInStock = false) 
        : IRequest<RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>>;
    
    public class GetFilteredProductsForCustomerQueryHandler(IRepository<Product> repository, IMediator mediator) 
        : IRequestHandler<GetFilteredProductsForCustomerQuery, RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>>
    {
        private readonly IRepository<Product> _repository = repository;
        private readonly IMediator _mediator = mediator;

        public async Task<RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>> Handle(GetFilteredProductsForCustomerQuery request, CancellationToken cancellationToken)
        {
            string? targetBusinessType = null;

            if (request.MatchCustomerBusinessType && !string.IsNullOrWhiteSpace(request.CustomerId))
            {
               var isCustomerExist=  await _mediator.Send(new GetCustomerByIdQuery(request.CustomerId));
                if (!isCustomerExist.isSuccess)
                {
                    return RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>.Failure(isCustomerExist.errorCode, isCustomerExist.message);
                }

                targetBusinessType = isCustomerExist.data.BusinessType;
            }
            else if (!string.IsNullOrWhiteSpace(request.BusinessType))
            {
                targetBusinessType = request.BusinessType;
            }

            var customerWishlistProductIds = new List<int>();
            if (!string.IsNullOrWhiteSpace(request.CustomerId))
            {
                var wishlistResult = await _mediator.Send(new GetWishlistProductsQuery(request.CustomerId), cancellationToken);
                customerWishlistProductIds = wishlistResult.isSuccess 
                    ? wishlistResult.data.Select(w => w.ProductId).ToList() 
                    : new List<int>();
            }

            // Build the query using repository
            var query = _repository.Get(p => !p.Deleted && p.Supplier != null)
                .Include(p => p.Supplier)
                    .ThenInclude(s => s.Rate)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .AsQueryable();

            // Apply business type filter
            if (!string.IsNullOrWhiteSpace(targetBusinessType))
            {
                query = query.Where(p => p.Supplier.BusinessType != null &&
                                       p.Supplier.BusinessType.Equals(targetBusinessType, StringComparison.OrdinalIgnoreCase));
            }

            // Apply category filter
            if (request.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == request.CategoryId.Value);
            }

            // Apply price filters
            if (request.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= request.MaxPrice.Value);
            }

            // Apply stock filter
            if (request.OnlyInStock)
            {
                query = query.Where(p => p.Stock > p.MinimumStock);
            }

            // Get the data first, then apply supplier rating filter in memory if needed
            var productsData = await query.ToListAsync(cancellationToken);

            var filteredProducts = productsData.AsEnumerable();

            // Apply supplier rating filter if specified
            if (request.MinSupplierRating.HasValue)
            {
                filteredProducts = filteredProducts.Where(p => 
                    p.Supplier.Rate.Any() && 
                    p.Supplier.Rate.Average(r => r.RateNumber) >= request.MinSupplierRating.Value);
            }

            var products = filteredProducts
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

           

            var result = products.ToList();

            if (result.IsNullOrEmpty())
            {
                return RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>.Failure(ErrorCode.NotFound, "No products found matching the specified criteria.");
            }

            return RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>.Success(result, "Products retrieved successfully.");
        }
    }
}
