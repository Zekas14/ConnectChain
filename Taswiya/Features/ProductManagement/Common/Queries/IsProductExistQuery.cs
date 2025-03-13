using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Product.GetProduct;
using MediatR;

namespace ConnectChain.Features.ProductManagement.Common.Queries
{
    public record IsProductExistQuery(int Id) : IRequest<RequestResult<Product>>;
    public class IsProductExistQueryHandler : IRequestHandler<IsProductExistQuery, RequestResult<Product>>
    {
        private readonly IRepository<Product> _repository;

        public IsProductExistQueryHandler(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task<RequestResult<Product>> Handle(IsProductExistQuery request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIDAsync(request.Id);
            if (product == null)
            {
                return RequestResult<Product>.Failure(ErrorCode.NotFound, "Product not found");
            }

            return RequestResult<Product>.Success(product);
        }
    }
}
