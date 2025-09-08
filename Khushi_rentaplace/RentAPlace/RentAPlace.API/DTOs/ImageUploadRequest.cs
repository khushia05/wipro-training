using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RentAPlace.API.DTOs
{
    public class ImageUploadRequest
    {
        [FromForm(Name = "image")]
        public IFormFile Image { get; set; }
    }

    public class MultipleImageUploadRequest
    {
        [FromForm(Name = "images")]
        public List<IFormFile> Images { get; set; }
    }
}
