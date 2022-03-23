using Microsoft.AspNetCore.Http;

namespace FullFraim.Models.ViewModels
{
    public class ImageInputUploadDemo
    {
        public IFormFile Image { get; set; }
        public string ImageUrl { get; set; }
    }
}
