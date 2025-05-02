using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;

namespace ConnectChain.Features.CategoryManagement.Common.Queries
{
    public record IsCategoryExistQuery<entity>(int CategoryId) : IRequest<RequestResult<bool>> where entity :BaseModel;
    public class IsCategoryExistQueryHandler<entity>(IRepository<entity> repository) : IRequestHandler<IsCategoryExistQuery<entity>, RequestResult<bool>>
    {
        private readonly IRepository<entity> repository = repository;



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
