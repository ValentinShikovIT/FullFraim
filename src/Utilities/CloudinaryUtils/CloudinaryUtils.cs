using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Utilities.CloudinaryUtils
{
    public class CloudinaryUtils : ICloudinaryUtils
    {
        private readonly Account account;
        private readonly Cloudinary cloudinary;

        public CloudinaryUtils(string CloudName, string ApiKey, string ApiSecret)
        {
            this.account = new Account(CloudName, ApiKey, ApiSecret);

            this.cloudinary = new Cloudinary(account);
        }

        public string UploadImage(IFormFile file, string extention = ".png")
        {
            string filePath = Guid.NewGuid().ToString();

            if (file == null)
            {
                throw new ArgumentNullException();
            }

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(filePath + extention, file.OpenReadStream()),
                Overwrite = true,
            };

            var uploadResult = this.cloudinary.Upload(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new ArgumentException(); // couldn't upload image
            }

            return uploadResult.SecureUrl.AbsoluteUri;
        }

        public string UploadImage(MemoryStream file, string extention = ".png")
        {
            string filePath = Guid.NewGuid().ToString();

            if (file == null)
            {
                throw new ArgumentNullException();
            }

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(filePath + extention, file),
                Overwrite = true,
            };

            var uploadResult = this.cloudinary.Upload(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new ArgumentException(); // couldn't upload image
            }

            return uploadResult.SecureUrl.AbsoluteUri;
        }
    }
}
