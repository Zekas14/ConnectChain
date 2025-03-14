using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Product.GetProduct;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace ConnectChain.Features.ProductManagement.GetSupplierProducts.Queries
{
    public record GetSupplierProductsByPageQuery(string SupplierId , PaginationHelper PaginationParams)
        : IRequest<RequestResult<IReadOnlyList<GetProductResponseViewModel>>> ;

    public class GetSupplierProductsByPageQueryHandler(IRepository<Product> repository)

        : IRequestHandler<GetSupplierProductsByPageQuery, RequestResult<IReadOnlyList<GetProductResponseViewModel>>>
    {
        private readonly IRepository<Product> repository = repository;

        public async Task<RequestResult<IReadOnlyList<GetProductResponseViewModel>>> Handle(GetSupplierProductsByPageQuery request, CancellationToken cancellationToken)
        {
            var products = repository.GetByPage(request.PaginationParams)
                .Where(p => p.SupplierId == request.SupplierId)
                .Select(p => new GetProductResponseViewModel
                {
                    Id = p.ID,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                });
            if (!products.IsNullOrEmpty())
            {
                return RequestResult<IReadOnlyList<GetProductResponseViewModel>>.Success(products.ToList());
            }
            return RequestResult<IReadOnlyList<GetProductResponseViewModel>>.Failure(ErrorCode.NotFound, "No Products found ");
        }
    }
}
