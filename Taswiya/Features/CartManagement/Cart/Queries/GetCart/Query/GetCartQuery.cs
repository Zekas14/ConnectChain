using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.ViewModel.Cart.GetCartItems;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ConnectChain.Features.CartManagement.Cart.Queries.GetCart.Query
{
    public record GetCartQuery(string CustomerId) : IRequest<RequestResult<GetCartItemsResponseViewModel>>;

    public class GetCartQueryHandler(IRepository<Models.Cart> cartRepo, IMemoryCache memoryCache)
        : IRequestHandler<GetCartQuery, RequestResult<GetCartItemsResponseViewModel>>
    {
        private readonly IRepository<Models.Cart> _cartRepo = cartRepo;
        private readonly IMemoryCache memoryCache = memoryCache;

        public async Task<RequestResult<GetCartItemsResponseViewModel>> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"cart:{request.CustomerId}";

            if (memoryCache.TryGetValue(cacheKey, out GetCartItemsResponseViewModel? cachedCart))
            {
                return RequestResult<GetCartItemsResponseViewModel>.Success(cachedCart, "Cart retrieved from cache.");
            }

            var cart = await cartRepo.Get(c => c.CustomerId == request.CustomerId)
                .Include(c => c.Items)
                .ThenInclude(c => c.Product)
                .ThenInclude(p => p.Images)
                .Select(c => new GetCartItemsResponseViewModel
                {
                    Total = c.Items.Select(i => i.UnitPrice * i.Quantity).Sum(),
                    Items = c.Items.Select(i => new CartItemViewModel
                    {
                        ProductId = i.ProductId,
                        ProductName = i.Product.Name!,
                        ProductImage = i.Product.Images.FirstOrDefault()!.Url!,
                        MinimumOrder = i.Product.MinimumStock / 2,
                        Price = i.UnitPrice,
                        Quantity = i.Quantity
                    }).ToList(),
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (cart == null)
                return RequestResult<GetCartItemsResponseViewModel>.Failure(ErrorCode.NotFound, "No cart found for this customer");

            memoryCache.Set(cacheKey, cart, TimeSpan.FromMinutes(30));

            return RequestResult<GetCartItemsResponseViewModel>.Success(cart, "Cart retrieved successfully.");
        }
    }

}
