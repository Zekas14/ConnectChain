using ConnectChain.Features.ImageManagement.UploadImage.Command;
using ConnectChain.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConnectChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImageController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;
        [HttpPost("Upload Image")]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            var response = await mediator.Send(new UploadImageCommand(image));
            return response.isSuccess ? new SuccessResponseViewModel<string>(response.data)
                : new FailureResponseViewModel<string>(response.errorCode, response.message);
        }
    }
}
