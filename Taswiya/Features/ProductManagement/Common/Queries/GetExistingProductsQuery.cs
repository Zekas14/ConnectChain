using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;

namespace ConnectChain.Features.ProductManagement.Common.Queries
{
    public record GetExistingProductsQuery(List<int> ProductIds) : IRequest<RequestResult<List<Product>>>;

    public class GetExistingProductsQueryHandler(IRepository<Product> repository)
        : IRequestHandler<GetExistingProductsQuery, RequestResult<List<Product>>>
    {
        private readonly IRepository<Product> repository = repository;

        public async Task<RequestResult<List<Product>>> Handle(GetExistingProductsQuery request, CancellationToken cancellationToken)
        {
            var products =  repository.Get(p => request.ProductIds.Contains(p.ID));

            var foundIds = products.Select(p => p.ID);
            var missingIds = request.ProductIds.Except(foundIds).ToList();

            if (missingIds.Count != 0)
            {
                return RequestResult<List<Product>>.Failure(ErrorCode.NotFound, $"Missing products: {string.Join(", ", missingIds)}");
            }

            return RequestResult<List<Product>>.Success(products.ToList(), "Products retrieved successfully");
        }
    }

}
