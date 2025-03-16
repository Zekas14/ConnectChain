using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel;
using MediatR;

namespace ConnectChain.Features.CategoryManagement.AddCategory.Command
{
    public record AddCategoryCommand(string Name , string Description) : IRequest<RequestResult<bool>>;
    public class AddCategoryCommandHandler(IRepository<Category> repository) : IRequestHandler<AddCategoryCommand, RequestResult<bool>>
    {
        private readonly IRepository<Category> _repository = repository;

        public  async Task<RequestResult<bool>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)

        {
            var category = new Category
            {
                Name = request.Name,
                Description = request.Description
            };

             await _repository.AddAsync(category);
            await _repository.SaveChangesAysnc();
            return  RequestResult<bool>.Success(true, "Category added successfully");
        }
    }   
}
