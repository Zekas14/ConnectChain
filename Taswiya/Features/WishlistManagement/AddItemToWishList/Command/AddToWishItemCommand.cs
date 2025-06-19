using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.ProductManagement.Common.Queries;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.WishlistManagement.AddItemToWishList.Command
{
    public record AddToWishItemCommand(int ProductId , string CustomerId) : IRequest<RequestResult<bool>>;
    public class AddToWishItemCommandHandler(IRepository<WishlistItem> repository,IMediator mediator) : IRequestHandler<AddToWishItemCommand, RequestResult<bool>>
    {
        private readonly IRepository<WishlistItem> _repository = repository;
        private readonly IMediator _mediator = mediator;

        public async Task<RequestResult<bool>> Handle(AddToWishItemCommand request, CancellationToken cancellationToken)
        {
            
            var existingWishItem = await _repository.Get(w => w.ProductId == request.ProductId && w.CustomerId == request.CustomerId)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingWishItem != null)
                return RequestResult<bool>.Failure(ErrorCode.BadRequest, "Product is already in wishlist.");

            var wishItem = new WishlistItem
            {
                ProductId = request.ProductId,
                CustomerId = request.CustomerId
            };

            await _repository.AddAsync(wishItem);
            await _repository.SaveChangesAsync();
            return RequestResult<bool>.Success(true,"Added To WishList");
        }
        
    }
}
