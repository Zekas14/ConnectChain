using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.ProductManagement.Common.Queries;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.WishlistManagement.RemoveItemFromWishList.Command
{
    public record RemoveFromWishItemCommand(int ProductId, string CustomerId) : IRequest<RequestResult<bool>>;
    
    public class RemoveFromWishItemCommandHandler(IRepository<WishlistItem> repository, IMediator mediator) : IRequestHandler<RemoveFromWishItemCommand, RequestResult<bool>>
    {
        private readonly IRepository<WishlistItem> _repository = repository;
        private readonly IMediator _mediator = mediator;

        public async Task<RequestResult<bool>> Handle(RemoveFromWishItemCommand request, CancellationToken cancellationToken)
        {
            var productExists = await _mediator.Send(new IsProductExistQuery(request.ProductId), cancellationToken);
            if (!productExists.isSuccess)
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Product not found.");

            var wishItem = await _repository.Get(w => w.ProductId == request.ProductId && w.CustomerId == request.CustomerId)
                .FirstOrDefaultAsync(cancellationToken);

            if (wishItem == null)
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Item not found in wishlist.");
            _repository.HardDelete(wishItem);
            await _repository.SaveChangesAsync();
            
            return RequestResult<bool>.Success(true, "Removed From WishList");
        }
    }
}
