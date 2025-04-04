using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Category.GetCategory;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace ConnectChain.Features.CategoryManagement.GetCategories.Queries
{
    public record GetCategoriesQuery : IRequest<RequestResult<IReadOnlyList<GetCategoryResponseViewModel>>>;
    public class GetCategoriesQueryHandler(IRepository<Category> repository) : IRequestHandler<GetCategoriesQuery, RequestResult<IReadOnlyList<GetCategoryResponseViewModel>>>
    {
        private readonly IRepository<Category> repository = repository;

        public async Task<RequestResult<IReadOnlyList<GetCategoryResponseViewModel>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = repository.GetAll().Select(c=>new GetCategoryResponseViewModel
            {
                Description = c.Description,
                Name = c.Name ,
                Id = c.ID
            }).ToList();
            if (categories.IsNullOrEmpty())
            {
                return RequestResult<IReadOnlyList<GetCategoryResponseViewModel>>.Failure(ErrorCode.NotFound, message: "No Categories Founded");
            }
            return RequestResult<IReadOnlyList<GetCategoryResponseViewModel>>.Success(categories);

        }
    }
}
