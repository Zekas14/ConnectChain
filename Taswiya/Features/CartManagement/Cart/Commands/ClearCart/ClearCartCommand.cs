using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.CartManagement.Cart.Commands.ClearCart
{
    public record ClearCartCommand(string CustomerId) : IRequest<RequestResult<bool>>;

    public class ClearCartCommandHandler(IRepository<Models.Cart> repository, ICacheService cache)
        : IRequestHandler<ClearCartCommand, RequestResult<bool>>
    {
        private readonly IRepository<Models.Cart> repository = repository;
        private readonly ICacheService cache = cache;

        public async Task<RequestResult<bool>> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            var cart = repository.Get(c => c.CustomerId == request.CustomerId)
                .Include(c => c.Items)
                .AsTracking()
                .FirstOrDefault();

            if (cart == null)
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Cart not found.");

            cart.Items.Clear();
            await repository.SaveChangesAsync();

            return RequestResult<bool>.Success(true, "Cart cleared.");
        }
    }

}
