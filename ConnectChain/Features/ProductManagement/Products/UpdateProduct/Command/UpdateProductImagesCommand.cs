using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;

namespace ConnectChain.Features.ProductManagement.Products.UpdateProduct.Command
{
    public record UpdateProductImagesCommand(int ProductId, List<IFormFile> NewImages, string[] RemainingImages) : IRequest<RequestResult<bool>>;
    public class UpdateProductImagesCommandHandler(IRepository<Image> repository, CloudinaryService cloudinaryService) : IRequestHandler<UpdateProductImagesCommand, RequestResult<bool>>
    {
        private readonly IRepository<Image> repository = repository;
        private readonly CloudinaryService cloudinaryService = cloudinaryService;

        public async Task<RequestResult<bool>> Handle(UpdateProductImagesCommand request, CancellationToken cancellationToken)
        {
            var newImages = request.NewImages;
            if (newImages == null || newImages.Count == 0)
                return RequestResult<bool>.Success(true);
            var deletedImages = repository.Get(i => i.ProductId == request.ProductId
            && !request.RemainingImages.Contains(i.Url)).ToList();
            foreach (var img in deletedImages)
            {
                repository.Delete(img);
            }

            if (request.NewImages is { Count: > 0 })
            {
                foreach (var imageFile in request.NewImages)
                {
                    string url = await cloudinaryService.UploadImageAsync(imageFile);
                    var image = new Image
                    {
                        Url = url,
                        ProductId = request.ProductId
                    };
                    repository.Add(image);
                }
            }

            /*         List<string> urls= new();
            for (int i = 0; i < newImages.Count; i++)
            {
                string url = await cloudinaryService.UploadImageAsync(newImages[i]);
                Image image = new Image
                {
                    ID = images[i].ID,
                    Url = url,
                    ProductId = request.ProductId
                };
                repository.SaveInclude(images[i], [nameof(Image.Url)]);
            }
   */
            await repository.SaveChangesAsync();
            return RequestResult<bool>.Success(true, "Images Updated Sueccessfully");
        }
    }
}
