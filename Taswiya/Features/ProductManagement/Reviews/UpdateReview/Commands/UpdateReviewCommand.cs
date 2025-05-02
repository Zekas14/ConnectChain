using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.ProductManagement.Common.Queries;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;

namespace ConnectChain.Features.ProductManagement.Reviews.UpdateReview.Commands
{
    public record UpdateReviewCommand : IRequest<RequestResult<bool>>
    {
        public int Id { get; init; }
        public string Body { get; init; }
        public int Rate { get; init; }
        public int ProductId { get; init; }
        public string CustomerId { get; init; }
    }

    public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, RequestResult<bool>>
    {
        private readonly IRepository<Review> _reviewRepository;
        private readonly IMediator _mediator;

        public UpdateReviewCommandHandler(
            IRepository<Review> reviewRepository,
            IMediator mediator)
        {
            _reviewRepository = reviewRepository;
            _mediator = mediator;
        }

        public async Task<RequestResult<bool>> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            // Validate product existence
            var productExistResult = await _mediator.Send(new IsProductExistQuery(request.ProductId), cancellationToken);
            if (!productExistResult.isSuccess)
            {
                return RequestResult<bool>.Failure(productExistResult.errorCode, productExistResult.message);
            }

           
            // Get the existing review
            var review = await _reviewRepository.GetByIDAsync(request.Id);
            if (review == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "The specified review does not exist.");
            }

            return RequestResult<bool>.Success(true, "Review Updated Successfully");
        }
    }
}
