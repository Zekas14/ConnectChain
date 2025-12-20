using CloudinaryDotNet.Actions;
using ConnectChain.Features.CategoryManagement.AddCategory.Command;
using ConnectChain.Features.CategoryManagement.GetCategories.Queries;
using ConnectChain.Helpers;
using ConnectChain.ViewModel;
using ConnectChain.ViewModel.Category.AddCategory;
using ConnectChain.ViewModel.Category.GetCategory;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConnectChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetCategoriesQuery());
            
            return new SuccessResponseViewModel<IReadOnlyList<GetCategoryResponseViewModel>> (result.data, result.message);

        }
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryRequestViewModel model)
        {
            var result = await _mediator.Send(new AddCategoryCommand(model.Name, model.Description));
            return new SuccessResponseViewModel<bool>(result.data, result.message);
        }

    }
}
