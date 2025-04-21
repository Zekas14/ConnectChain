using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.ProductManagement.Common.Queries;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;

namespace ConnectChain.Features.ProductManagement.AddProduct.Command
{
    public record AddProductImageCommand(IFormFile Image, int ProductId) : IRequest<RequestResult<bool>>;
    public class AddProductImageCommandHandler(IRepository<Image> repository, IMediator mediator, CloudinaryService cloudinaryService) : IRequestHandler<AddProductImageCommand, RequestResult<bool>>
    {
        private readonly IRepository<Image> repository = repository;
        private readonly IMediator mediator = mediator;
        private readonly CloudinaryService cloudinaryService = cloudinaryService;

        public async Task<RequestResult<bool>> Handle(AddProductImageCommand request, CancellationToken cancellationToken)
        {
            var productExistResult = await mediator.Send(new IsProductExistQuery(request.ProductId), cancellationToken);
            if (!productExistResult.isSuccess)
            {
                return RequestResult<bool>.Failure(productExistResult.errorCode, productExistResult.message);
            }
            var result = await cloudinaryService.UploadImageAsync(request.Image);
            if (result != null)
            {
                var image = new Image
                {
                    Url = result,
                    ProductId = request.ProductId
                };
                repository.Add(image);
                await repository.SaveChangesAsync();
                return RequestResult<bool>.Success(true);
            }
            return RequestResult<bool>.Failure(ErrorCode.InternalServerError, "Failed to upload image");
        }
    }
    
}
