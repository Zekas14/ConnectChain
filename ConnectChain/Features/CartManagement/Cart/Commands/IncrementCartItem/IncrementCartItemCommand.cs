using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.ProductManagement.Common.Queries;
using ConnectChain.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.CartManagement.Cart.Commands.IncrementCartItem
{
    public record IncrementCartItemCommand(string CustomerId, int ProductId) : IRequest<RequestResult<bool>>;

    public class IncrementCartItemCommandHandler(IRepository<Models.Cart> repository, IMediator mediator)
        : IRequestHandler<IncrementCartItemCommand, RequestResult<bool>>
    {
        private readonly IRepository<Models.Cart> _repository = repository;
        private readonly IMediator _mediator = mediator;

        public async Task<RequestResult<bool>> Handle(IncrementCartItemCommand request, CancellationToken cancellationToken)
        {
            var cart = await _repository.Get(
                c => c.CustomerId == request.CustomerId)
                .Include(c=>c.Items)
                .AsTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (cart == null)
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Cart not found.");
            var product = await _mediator.Send(new IsProductExistQuery(request.ProductId), cancellationToken);
            if (!product.isSuccess)
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Product not found.");


            var item = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (item == null)
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Product not in cart.");

            if (item.Quantity + 1 > product.data.Stock)
                return RequestResult<bool>.Failure(ErrorCode.InvalidInput, "No more stock available.");

            item.Quantity += 1;
            await _repository.SaveChangesAsync();

            return RequestResult<bool>.Success(true, "Product quantity increased.");
        }
    }
}
