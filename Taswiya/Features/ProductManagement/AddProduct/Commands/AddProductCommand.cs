using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.CategoryManagement.Common.Queries;
using ConnectChain.Features.ImageManagement.UploadImage.Command;
using ConnectChain.Features.SupplierManagement.Common.Queries;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;

namespace ConnectChain.Features.ProductManagement.AddProduct.Command
{
    public record AddProductCommand : IRequest<RequestResult<bool>>
    {
        public string? Name { get; init; }
        public string? Description { get; init; }
        public List<IFormFile>? Images { get; init; }
        public decimal Price { get; init; }
        public int? Stock { get; init; }    
        public string? SupplierId { get; init; }
        public int MinimumStock { get; init; }
        public int CategoryId { get; init; }
    }
    public class AddProductCommandHandler(IRepository<Product> repository, IMediator mediator) : IRequestHandler<AddProductCommand, RequestResult<bool>>
    {
        private readonly IRepository<Product> repository = repository;
        private readonly IMediator mediator = mediator;


        public async Task<RequestResult<bool>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var categoryExistResult = await mediator.Send(new IsCategoryExistQuery(request.CategoryId));
            var isUserExistResult = await mediator.Send(new IsSupplierExistsQuery(request.SupplierId));
            if (!categoryExistResult.isSuccess)
            {
                return RequestResult<bool>.Failure(categoryExistResult.errorCode, categoryExistResult.message);
            }
            if (!isUserExistResult.isSuccess)
            {
                return RequestResult<bool>.Failure(isUserExistResult.errorCode, isUserExistResult.message);
            }
            List<Image> images = new List<Image>();
            foreach(var image in request.Images)
            {
                var uploadImageResult = await mediator.Send(new UploadImageCommand(image));
                if (!uploadImageResult.isSuccess)
                {
                    return RequestResult<bool>.Failure(uploadImageResult.errorCode, uploadImageResult.message);
                }
                images.Add(new Image()
                {
                    Url = uploadImageResult.data
                });
            }
            var product = new Product
            {

                Name = request.Name,
                Images = images,
                SKU = Guid.NewGuid(),
                MinimumStock = request.MinimumStock,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock,
                SupplierId = request.SupplierId,
                CategoryId = request.CategoryId
            };
            repository.Add(product);
            await repository.SaveChangesAsync();
            return RequestResult<bool>.Success(true,"Product Added Successfully");
        }
    }
}
