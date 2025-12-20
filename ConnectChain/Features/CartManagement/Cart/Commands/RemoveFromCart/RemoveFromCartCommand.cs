using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.CartManagement.Cart.Commands.RemoveFromCart
{
    public record RemoveCartItemCommand(string CustomerId, int ProductId) : IRequest<RequestResult<bool>>;

    public class RemoveCartItemCommandHandler(IRepository<Models.Cart> repository, ICacheService cache)
        : IRequestHandler<RemoveCartItemCommand, RequestResult<bool>>
    {
        private readonly IRepository<Models.Cart> repository = repository;
        private readonly ICacheService cache = cache;

        public async Task<RequestResult<bool>> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
        {
            var cart = repository.Get(c => c.CustomerId == request.CustomerId)
                .Include(c => c.Items)
                .AsTracking()
                .FirstOrDefault();

            if (cart == null)
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Cart not found.");

            var item = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (item == null)
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Item not found.");

            cart.Items.Remove(item);
            await repository.SaveChangesAsync();

            return RequestResult<bool>.Success(true, "Item removed from cart.");
        }
    }

}
