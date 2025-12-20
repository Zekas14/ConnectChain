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
    public record GetProductsByCategoryQuery(int CategoryId, string? CustomerId = null) 
        : IRequest<RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>>;
    
    public class GetProductsByCategoryQueryHandler(IRepository<Product> repository, IMediator mediator) 
        : IRequestHandler<GetProductsByCategoryQuery, RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>>
    {
        private readonly IRepository<Product> _repository = repository;
        private readonly IMediator _mediator = mediator;

        public async Task<RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            var isCategoryExist = await _mediator.Send(new IsCategoryExistQuery(request.CategoryId),cancellationToken);
            if(!isCategoryExist.isSuccess)
            {
                return RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>.Failure(isCategoryExist.errorCode,isCategoryExist.message);
            }

            // Get customer's wishlist using the existing wishlist query if customer ID is provided
            var customerWishlistProductIds = new List<int>();
            if (!string.IsNullOrWhiteSpace(request.CustomerId))
            {
                var wishlistResult = await _mediator.Send(new GetWishlistProductsQuery(request.CustomerId), cancellationToken);
                customerWishlistProductIds = wishlistResult.isSuccess 
                    ? wishlistResult.data.Select(w => w.ProductId).ToList() 
                    : new List<int>();
            }

            // Get products by category using repository
            var query = _repository.Get(p => !p.Deleted && 
                                            p.CategoryId == request.CategoryId &&
                                            p.Supplier != null)
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
                return RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>.Failure(ErrorCode.NotFound, $"No products found in category with ID {request.CategoryId}.");
            }

            return RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>.Success(products, "Products retrieved successfully.");
        }
    }
}
