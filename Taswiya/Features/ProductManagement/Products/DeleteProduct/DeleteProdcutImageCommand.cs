using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using MediatR;
using ConnectChain.Models;
using Microsoft.EntityFrameworkCore;
namespace ConnectChain.Features.ProductManagement.Products.DeleteProduct
{
    public record DeleteProdcutImageCommand(int Id) : IRequest<RequestResult<bool>>;
    public class DeleteProductImageCommandHandler(IRepository<Image> repository) : IRequestHandler<DeleteProdcutImageCommand, RequestResult<bool>>
    {
        private readonly IRepository<Image> repository = repository;

        public async Task<RequestResult<bool>> Handle(DeleteProdcutImageCommand request, CancellationToken cancellationToken)
        {
            var image = await repository.GetByIDAsync(request.Id);
            if (image is null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Image not found");
            }
            repository.Delete(image);
            await repository.SaveChangesAsync();
            return RequestResult<bool>.Success(true, "Image Deleted Successfully");
        }
    }
}
