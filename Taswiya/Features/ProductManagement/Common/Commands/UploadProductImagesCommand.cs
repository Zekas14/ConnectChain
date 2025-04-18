using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.ImageManagement.UploadImage.Command;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;

namespace ConnectChain.Features.ProductManagement.Common.Commands
{
    public record UploadProductImagesCommand(ICollection<IFormFile> Images,int ProductId): IRequest<RequestResult<List<Image>>>;
    public class UploadProductImagesHandler(IRepository<Image> repository, CloudinaryService cloudinaryService ) 
        : IRequestHandler<UploadProductImagesCommand, RequestResult<List<Image>>>
    {
        private readonly IRepository<Image> repository = repository;
        private readonly CloudinaryService cloudinaryService = cloudinaryService;

        public async Task<RequestResult<List<Image>>> Handle(UploadProductImagesCommand request, CancellationToken cancellationToken)
        {
            List<Image> images = [];
            foreach (var image in request.Images)
            {
                string url = await cloudinaryService.UploadImageAsync(image);
                images.Add(new Image()
                {
                    Url = url,
                    ProductId = request.ProductId
                });
            }
            repository.AddRange(images);
            await repository.SaveChangesAysnc();
            return RequestResult<List<Image>>.Success(images, "Images Uploaded Successfully");

        }
    }
}
