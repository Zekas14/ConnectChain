using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Wishlist.GetWishlistProducts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ConnectChain.Features.WishlistManagement.GetWishlistProducts.Queries
{
    public record GetWishlistProductsQuery(string CustomerId) : IRequest<RequestResult<IReadOnlyList<GetWishlistProductsResponseViewModel>>>;
    
    public class GetWishlistProductsQueryHandler(IRepository<WishlistItem> repository) 
        : IRequestHandler<GetWishlistProductsQuery, RequestResult<IReadOnlyList<GetWishlistProductsResponseViewModel>>>
    {
        private readonly IRepository<WishlistItem> _repository = repository;

        public async Task<RequestResult<IReadOnlyList<GetWishlistProductsResponseViewModel>>> Handle(GetWishlistProductsQuery request, CancellationToken cancellationToken)
        {
            var wishlistProducts = await _repository.Get(w => w.CustomerId == request.CustomerId)
                .Include(w => w.Product)
                    .ThenInclude(p => p.Images)
                .Include(w => w.Product)
                    .ThenInclude(p => p.Category)
                .Select(w => new GetWishlistProductsResponseViewModel
                {
                    ProductId = w.ProductId,
                    Name = w.Product.Name,
                    Price = w.Product.Price,
                    Stock = w.Product.Stock,
                    CategoryName = w.Product.Category != null ? w.Product.Category.Name : null,
                    Image = w.Product.Images.Where(i => !i.Deleted).Select(i => i.Url).FirstOrDefault() ?? string.Empty,
                    IsStockAvailable = w.Product.MinimumStock < w.Product.Stock,
                    AddedToWishlistDate = w.CreatedDate
                })
                .OrderByDescending(w => w.AddedToWishlistDate)
                .ToListAsync(cancellationToken);

            if (wishlistProducts.IsNullOrEmpty())
            {
                return RequestResult<IReadOnlyList<GetWishlistProductsResponseViewModel>>.Failure(ErrorCode.NotFound, "No products found in wishlist.");
            }

            return RequestResult<IReadOnlyList<GetWishlistProductsResponseViewModel>>.Success(wishlistProducts, "Wishlist products retrieved successfully.");
        }
    }
}
