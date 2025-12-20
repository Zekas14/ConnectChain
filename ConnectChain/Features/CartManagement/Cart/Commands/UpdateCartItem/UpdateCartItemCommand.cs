using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.ProductManagement.Common.Queries;
using ConnectChain.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.CartManagement.Cart.Commands.UpdateCartItem
{
    public record UpdateCartItemCommand(string CustomerId, int ProductId, int Quantity) : IRequest<RequestResult<bool>>;

    public class UpdateCartItemCommandHandler(IRepository<Models.Cart> repository, IMediator mediator, ICacheService cache)
        : IRequestHandler<UpdateCartItemCommand, RequestResult<bool>>
    {
        private readonly IRepository<Models.Cart> repository = repository;
        private readonly IMediator mediator = mediator;
        private readonly ICacheService cache = cache;

        public async Task<RequestResult<bool>> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
        {
            if (request.Quantity <= 0)
                return RequestResult<bool>.Failure(ErrorCode.InvalidInput, "Quantity must be greater than 0.");

            var productCheck = await mediator.Send(new IsProductExistQuery(request.ProductId), cancellationToken);
            if (!productCheck.isSuccess)
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Product not found.");

            var cart = repository.Get(c => c.CustomerId == request.CustomerId)
                .Include(c => c.Items)
                .AsTracking()
                .FirstOrDefault();

            if (cart == null)
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Cart not found.");

            var item = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (item == null)
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Item not found in cart.");

            if (request.Quantity > productCheck.data.Stock)
                return RequestResult<bool>.Failure(ErrorCode.InvalidInput, "Quantity exceeds stock.");

            item.Quantity = request.Quantity;

            await repository.SaveChangesAsync();

            return RequestResult<bool>.Success(true, "Cart item updated successfully.");
        }
    }

}
