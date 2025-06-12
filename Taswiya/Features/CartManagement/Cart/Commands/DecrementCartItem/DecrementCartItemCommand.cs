using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.CartManagement.Cart.Commands.DecrementCartItem
{
    public record DecrementCartItemCommand(string CustomerId, int ProductId) : IRequest<RequestResult<bool>>;

    public class DecrementCartItemCommandHandler(IRepository<Models.Cart> repository)
        : IRequestHandler<DecrementCartItemCommand, RequestResult<bool>>
    {
        private readonly IRepository<Models.Cart> _repository = repository;

        public async Task<RequestResult<bool>> Handle(DecrementCartItemCommand request, CancellationToken cancellationToken)
        {
            var cart = await _repository.Get(
                c => c.CustomerId == request.CustomerId
            ).Include(c=>c.Items)
            .FirstOrDefaultAsync(cancellationToken);

            if (cart == null)
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Cart not found.");

            var item = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (item == null)
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Product not in cart.");

            if (item.Quantity <= 1)
                return RequestResult<bool>.Failure(ErrorCode.InvalidInput, "Minimum quantity reached.");

            item.Quantity -= 1;
            await _repository.SaveChangesAsync();

            return RequestResult<bool>.Success(true, "Product quantity decreased.");
        }
    }

}
