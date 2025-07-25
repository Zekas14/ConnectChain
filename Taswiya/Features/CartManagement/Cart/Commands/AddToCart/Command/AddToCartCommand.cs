﻿using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.ProductManagement.Common.Queries;
using ConnectChain.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ConnectChain.Features.CartManagement.Cart.Commands.AddToCart.Command
{
    public record AddToCartCommand(string CustomerId, int ProductId, int Quantity) : IRequest<RequestResult<bool>>;

    public class AddToCartCommandHandler(
        IRepository<Models.Cart> repository,
        IMediator mediator,
        IMemoryCache cache)
    : IRequestHandler<AddToCartCommand, RequestResult<bool>>
    {
        private readonly IRepository<Models.Cart> _repository = repository;
        private readonly IMediator _mediator = mediator;    
        private readonly IMemoryCache _cache = cache;

        public async Task<RequestResult<bool>> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            if (request.Quantity <= 0)
                return RequestResult<bool>.Failure(ErrorCode.InvalidInput, "Quantity must be greater than 0.");

            var productExists = await _mediator.Send(new IsProductExistQuery(request.ProductId), cancellationToken);
            if (!productExists.isSuccess)
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Product Not Found");

            var existingCart = _repository.Get(c => c.CustomerId == request.CustomerId)
                    .Include(c => c.Items)
                    .AsTracking()
                    .FirstOrDefault();

                if (existingCart == null)
                {
                    existingCart = new Models.Cart
                    {
                        CustomerId = request.CustomerId,
                        Items = new List<Models.CartItem>()
                    };
                    _repository.Add(existingCart);
                }

            if (request.Quantity > productExists.data.Stock)
                return RequestResult<bool>.Failure(ErrorCode.InvalidInput, "Requested quantity exceeds stock.");

            var existingItem = existingCart!.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
            }
            else
            {
                existingCart.Items.Add(new Models.CartItem
                {
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    UnitPrice = productExists.data.Price*request.Quantity
                });

            }
            await _repository.SaveChangesAsync();


            return RequestResult<bool>.Success(true, "Product added to cart.");
        }
    }
}
