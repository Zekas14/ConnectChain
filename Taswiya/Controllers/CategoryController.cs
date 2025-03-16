using CloudinaryDotNet.Actions;
using ConnectChain.Features.CategoryManagement.AddCategory.Command;
using ConnectChain.Helpers;
using ConnectChain.ViewModel;
using ConnectChain.ViewModel.Category.AddCategory;
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

        [HttpPost("AddCategory")]
        public async Task<ResponseViewModel<bool>> AddCategory([FromBody] AddCategoryRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new AddCategoryCommand(model.Name, model.Description));
                return new SuccessResponseViewModel<bool>(result.data, result.message);
            }
            var errors = string.Join(", ", ModelState.Select(e => e.Value!.Errors));
            return new FaluireResponseViewModel<bool>(ErrorCode.BadRequest, errors);
        }
    }
}
