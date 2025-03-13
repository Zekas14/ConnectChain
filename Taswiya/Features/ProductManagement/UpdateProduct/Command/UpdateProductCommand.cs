﻿using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.CategoryManagement.Common.Queries;
using ConnectChain.Features.ProductManagement.Common.Queries;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;

namespace ConnectChain.Features.ProductManagement.UpdateProduct.Command
{
    public record UpdateProductCommand : IRequest<RequestResult<bool>>
    {
        public int ProductId { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public string? Image { get; init; }
        public decimal Price { get; init; }
        public int? Stock { get; init; }
        public int CategoryId { get; init; }
    }
    public class UpdateProductCommandHandler(IRepository<Product> repository, IMediator mediator) : IRequestHandler<UpdateProductCommand, RequestResult<bool>>
    {
        private readonly IRepository<Product> repository = repository;
        private readonly IMediator mediator = mediator;

        public async Task<RequestResult<bool>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var productExistResult = await mediator.Send(new IsProductExistQuery(request.ProductId), cancellationToken);
            if (!productExistResult.isSuccess)
            {
                return RequestResult<bool>.Failure(productExistResult.errorCode, productExistResult.message);
            }
            var categoryExistResult = await mediator.Send(new IsCategoryExistQuery(request.CategoryId), cancellationToken);
            if (!categoryExistResult.isSuccess)
            {
                return RequestResult<bool>.Failure(categoryExistResult.errorCode, categoryExistResult.message);
            }
            var product = new Product()
            {
                ID =request.ProductId ,
                Name = request.Name,
                Description = request.Description,
                Image = request.Image,
                Price = request.Price,
                Stock = request.Stock,
                CategoryId = request.CategoryId
            };
            /*
            productExistResult.data.Name = request.Name;
            productExistResult.data.Description = request.Description;
            productExistResult.data.Image = request.Image;
            productExistResult.data.Price = request.Price;
            productExistResult.data.Stock = request.Stock;
            productExistResult.data.CategoryId = request.CategoryId;*/
            string[] properties = { "Name", "Description", "Image", "Price", "Stock", "CategoryId" };
            repository.SaveInclude(product,properties);
            await repository.SaveChangesAysnc();
            return RequestResult<bool>.Success(true);
        }
    }   
}
