using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using crud_application.Models;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace crud_application.service
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        //private readonly HttpClient _httpClient;
        public CloudinaryService(IOptions<CloudinarySettings> config)
        {

            //_httpClient = httpClient;
            //_httpClient.Timeout = TimeSpan.FromMinutes(5);

            var settings = config.Value;
            var account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);
            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }

        public async Task<ImageUploadResult> UploadImageAsync(Stream imageStream, string fileName)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, imageStream),
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true
            };

            return await _cloudinary.UploadAsync(uploadParams);
        }

        public async Task<DeletionResult> DestroyAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);
            return result;
        }
    }
}