using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;

namespace ConnectChain.Features.CategoryManagement.Common.Queries
{
    public record IsCategoryExistQuery(int CategoryId) : IRequest<RequestResult<bool>> ;
    public class IsCategoryExistQueryHandler(IRepository<Category> repository) : IRequestHandler<IsCategoryExistQuery, RequestResult<bool>>
    {
        private readonly IRepository<Category> repository = repository;



        public async Task<RequestResult<bool>> Handle(IsCategoryExistQuery request, CancellationToken cancellationToken)
        {
            var category = await repository.GetByIDAsync(request.CategoryId);
            if (category == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Category not found");
            }
            return RequestResult<bool>.Success(true);
        }
    }
}
