using ConnectChain.Features.ProductManagement.Reviews.AddReview.Command;
using ConnectChain.Features.ProductManagement.Reviews.UpdateReview.Commands;
using ConnectChain.Filters;
using ConnectChain.Helpers;
using ConnectChain.Models.Enums;
using ConnectChain.ViewModel;
using ConnectChain.ViewModel.Review.AddReview;
using ConnectChain.ViewModel.Review.UpdateReview;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConnectChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorization(Role.Customer)]
    public class ReviewController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
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
        [HttpPut("UpdateReview")]
        public async Task<ResponseViewModel<bool>> UpdateReview(UpdateReviewRequestViewModel viewModel)
        {
            var result = await _mediator.Send(new UpdateReviewCommand
            {
                Id = viewModel.Id,
                Body = viewModel.Body,
                Rate = viewModel.Rate,
            });
            return result.isSuccess
                ? new SuccessResponseViewModel<bool>(result.data, result.message)
                : new FailureResponseViewModel<bool>(result.errorCode, result.message);
        }

    }
}
