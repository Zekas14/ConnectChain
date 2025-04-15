using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ConnectChain.Settings;
using Microsoft.Extensions.Options;

namespace ConnectChain.Helpers
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService(IOptions<CloudinarySettings> options)
        {
            var cloudinarySettings = options.Value;
            Account account = new Account(cloudinarySettings.CloudName, cloudinarySettings.ApiKey, cloudinarySettings.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file.Length == 0)
                return null;

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.Error != null)
            {
                throw new Exception($"Error uploading image: {uploadResult.Error.Message}");
            }
            return uploadResult.Url.ToString();
        }
    }
}
