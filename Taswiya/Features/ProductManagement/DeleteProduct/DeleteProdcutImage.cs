using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using MediatR;
using ConnectChain.Models;
namespace ConnectChain.Features.ProductManagement.DeleteProduct
{
    public record DeleteProdcutImage(int ImageId) : IRequest<RequestResult<bool>>;
    public class DeleteProductImageCommandHandler(IRepository<Image> repository) : IRequestHandler<DeleteProdcutImage, RequestResult<bool>>
    {
        private readonly IRepository<Image> repository = repository;

        public async Task<RequestResult<bool>> Handle(DeleteProdcutImage request, CancellationToken cancellationToken)
        {
            var image = await repository.GetByIDAsync(request.ImageId);
            if (image is null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Image not found");
            }
            repository.Delete(image);
            await repository.SaveChangesAysnc();
            return RequestResult<bool>.Success(true);
        }
    }
}
