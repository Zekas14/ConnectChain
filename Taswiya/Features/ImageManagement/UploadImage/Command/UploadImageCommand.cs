using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.Settings;
using MediatR;

namespace ConnectChain.Features.ImageManagement.UploadImage.Command
{
    public record UploadImageCommand(IFormFile Image): IRequest<RequestResult<string>>;
    public class UploadImageCommandHandler(CloudinaryService cloudinaryService,IRepository<Image> repository) : IRequestHandler<UploadImageCommand, RequestResult<string>>
    {
        private readonly CloudinaryService cloudinaryService = cloudinaryService;
        private readonly IRepository<Image> repository = repository;

        public async Task<RequestResult<string>> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            var result = await cloudinaryService.UploadImageAsync(request.Image);
            if (result != null)
            {
                var image = new Image () { Url = result };
                repository.Add(image);
                return RequestResult<string>.Success(result);
            }
            return RequestResult<string>.Failure(ErrorCode.InternalServerError, "Failed to upload image");
        }
    }
}
