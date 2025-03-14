using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.CategoryManagement.Common.Queries;
using ConnectChain.Features.ImageManagement.UploadImage.Command;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;

namespace ConnectChain.Features.ProductManagement.AddProduct.Command
{
    public record AddProductCommand : IRequest<RequestResult<bool>>
    {
        public string? Name { get; init; }
        public string? Description { get; init; }
        public IFormFile? Image { get; init; }
        public decimal Price { get; init; }
        public int? Stock { get; init; }
        public string? SupplierId { get; init; }
        public int CategoryId { get; init; }
    }
    public class AddProductCommandHandler(IRepository<Product> repository, IMediator mediator) : IRequestHandler<AddProductCommand, RequestResult<bool>>
    {
        private readonly IRepository<Product> repository = repository;
        private readonly IMediator mediator = mediator;

        public string ImageUrl { get; private set; }

        public async Task<RequestResult<bool>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var categoryExistResult = await mediator.Send(new IsCategoryExistQuery(request.CategoryId));
            if (!categoryExistResult.isSuccess)
            {
                return RequestResult<bool>.Failure(categoryExistResult.errorCode, categoryExistResult.message);
            }
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock,
                SupplierId = request.SupplierId,
                CategoryId = request.CategoryId
            };
            repository.Add(product);
            await repository.SaveChangesAysnc();
            return RequestResult<bool>.Success(true);
        }
    }
}
