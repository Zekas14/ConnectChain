using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.ProductManagement.Common.Queries;
using ConnectChain.Helpers;
using ConnectChain.Models;

using MediatR;

namespace ConnectChain.Features.ProductManagement.Reviews.AddReview.Command
{
    public record AddReviewCommand : IRequest<RequestResult<bool>>
    {
        public string Body { get; init; }
        public int Rate { get; init; }
        public int ProductId { get; init; }
        public string CustomerId { get; init; }
    }

    public class AddReviewCommandHandler : IRequestHandler<AddReviewCommand, RequestResult<bool>>
    {
        private readonly IRepository<Review> _reviewRepository;
        private readonly IMediator _mediator;

        public AddReviewCommandHandler(
            IRepository<Review> reviewRepository,
            IMediator mediator)
        {
            _reviewRepository = reviewRepository;
            _mediator = mediator;
        }

        public async Task<RequestResult<bool>> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
          

            // Validate customer existence
           /* var customerExistResult = await _mediator.Send(new IsCustomerExistQuery(request.CustomerId), cancellationToken);
            if (!customerExistResult.isSuccess)
            {
                return RequestResult<bool>.Failure(customerExistResult.errorCode, customerExistResult.message);
            }*/

            var review = new Review
            {
                Body = request.Body,
                Rate = request.Rate,
                ProductId = request.ProductId,
                CustomerId = request.CustomerId
            };

            _reviewRepository.Add(review);
            await _reviewRepository.SaveChangesAsync();

            return RequestResult<bool>.Success(true, "Review Added Successfully");
        }
    }
}
