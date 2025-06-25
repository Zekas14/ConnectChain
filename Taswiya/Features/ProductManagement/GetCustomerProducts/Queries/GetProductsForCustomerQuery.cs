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
    public record GetProductsForCustomerQuery(string CustomerId) 
        : IRequest<RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>>;
    
    public class GetProductsForCustomerQueryHandler(IRepository<Product> repository, IMediator mediator) 
        : IRequestHandler<GetProductsForCustomerQuery, RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>>
    {
        private readonly IRepository<Product> _repository = repository;
        private readonly IMediator _mediator = mediator;

        public async Task<RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>> Handle(GetProductsForCustomerQuery request, CancellationToken cancellationToken)
        {
            var isCustomerExist = await _mediator.Send(new GetCustomerByIdQuery(request.CustomerId));
            if (!isCustomerExist.isSuccess)
            {
                return RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>.Failure(isCustomerExist.errorCode, isCustomerExist.message);
            }


            if (string.IsNullOrWhiteSpace(isCustomerExist.data.BusinessType))
            {
                return RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>.Failure(ErrorCode.BadRequest, "Customer business type is not set.");
            }

            var wishlistResult = await _mediator.Send(new GetWishlistProductsQuery(request.CustomerId), cancellationToken);
            var customerWishlistProductIds = wishlistResult.isSuccess 
                ? wishlistResult.data.Select(w => w.ProductId).ToList() 
                : new List<int>();

            var query = _repository.Get(p => p.Supplier.BusinessType.ToLower() == isCustomerExist.data.BusinessType.ToLower())
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
                return RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>.Failure(ErrorCode.NotFound, $"No products found from suppliers with business type '{isCustomerExist.data.BusinessType}'.");
            }

            return RequestResult<IReadOnlyList<GetCustomerProductsResponseViewModel>>.Success(products, "Products retrieved successfully.");
        }
    }
}
