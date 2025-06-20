using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.CategoryManagement.Common.Queries;
using ConnectChain.Features.WishlistManagement.GetWishlistProducts.Queries;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Product.GetCustomerProducts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ConnectChain.Features.ProductManagement.GetProductsByCategory.Queries
{
    public record GetFilteredProductsByCategoryQuery(
        int CategoryId,
        string? CustomerId = null,
        string? BusinessType = null,
        decimal? MinPrice = null,
        decimal? MaxPrice = null,
        double? MinSupplierRating = null,
        bool OnlyInStock = false) 
        : IRequest<RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>>;
    
    public class GetFilteredProductsByCategoryQueryHandler(IRepository<Product> repository, IMediator mediator) 
        : IRequestHandler<GetFilteredProductsByCategoryQuery, RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>>
    {
        private readonly IRepository<Product> _repository = repository;
        private readonly IMediator _mediator = mediator;

        public async Task<RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>> Handle(GetFilteredProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            var isCategoryExist = await _mediator.Send(new IsCategoryExistQuery(request.CategoryId));
            if (!isCategoryExist.isSuccess)
            {
                return RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>.Failure(isCategoryExist.errorCode, isCategoryExist.message);
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
                                            p.CategoryId == request.CategoryId &&
                                            p.Supplier != null)
                .Include(p => p.Supplier)
                    .ThenInclude(s => s.Rate)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.BusinessType))
            {
                query = query.Where(p => p.Supplier.BusinessType != null &&
                                       p.Supplier.BusinessType.Equals(request.BusinessType, StringComparison.OrdinalIgnoreCase));
            }

            if (request.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= request.MaxPrice.Value);
            }

            if (request.OnlyInStock)
            {
                query = query.Where(p => p.Stock > p.MinimumStock);
            }

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
                return RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>.Failure(ErrorCode.NotFound, $"No products found in category with ID {request.CategoryId} matching the specified criteria.");
            }

            return RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>.Success(result, "Products retrieved successfully.");
        }
    }
}
