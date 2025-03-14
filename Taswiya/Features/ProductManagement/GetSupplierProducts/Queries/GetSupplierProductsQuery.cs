using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel;
using ConnectChain.ViewModel.Product.GetProduct;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace ConnectChain.Features.ProductManagement.GetSupplierProducts.Queries
{
    public record GetSupplierProductsQuery(string SupplierId) : IRequest<RequestResult<IReadOnlyList<GetProductResponseViewModel>>>;
    public class GetSupplierProductsQueryHandler(IRepository<Product> repository)
        : IRequestHandler<GetSupplierProductsQuery, RequestResult<IReadOnlyList<GetProductResponseViewModel>>>
    {
        private readonly IRepository<Product> repository = repository;

        public async Task<RequestResult<IReadOnlyList<GetProductResponseViewModel>>> Handle(GetSupplierProductsQuery request, CancellationToken cancellationToken)
        {
            var products =  repository.Get(p => p.SupplierId == request.SupplierId)
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
