using ConnectChain.Features.ProductManagement.Reviews.AddReview.Command;
using ConnectChain.Filters;
using ConnectChain.Helpers;
using ConnectChain.Models.Enums;
using ConnectChain.ViewModel;
using ConnectChain.ViewModel.Review.AddReview;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConnectChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPost]
        [Authorization(Role.Customer)]

        [HttpPost("AddReview")]
        public async Task<ResponseViewModel<bool>> AddReview(AddReviewRequestViewModel viewModel)
        {
            var customerId = Request.GetIdFromToken();
            var result = await _mediator.Send(new AddReviewCommand
            {
                Body = viewModel.Body,
                Rate = viewModel.Rate,
                ProductId = viewModel.ProductId,
                CustomerId = customerId
            });
            return result.isSuccess
                ? new SuccessResponseViewModel<bool>(result.data, result.message)
                : new FailureResponseViewModel<bool>(result.errorCode, result.message);
        }

    }
}
