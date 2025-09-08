using Microsoft.AspNetCore.Mvc;
using RentAPlace.API.DTOs;

namespace RentAPlace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageUploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ImageUploadController> _logger;

        public ImageUploadController(IWebHostEnvironment environment, ILogger<ImageUploadController> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        // Single Image Upload
        [HttpPost("upload-single")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage([FromForm] ImageUploadRequest request)
        {
            if (request.Image == null || request.Image.Length == 0)
                return BadRequest("No image uploaded.");

            var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Image.FileName)}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.Image.CopyToAsync(stream);
            }

            return Ok(new { fileName, url = $"/uploads/{fileName}" });
        }

        // Multiple Image Upload
        [HttpPost("upload-multiple")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadMultipleImages([FromForm] MultipleImageUploadRequest request)
        {
            if (request.Images == null || !request.Images.Any())
                return BadRequest("No images uploaded.");

            var uploaded = new List<object>();
            var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            foreach (var img in request.Images)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(img.FileName)}";
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await img.CopyToAsync(stream);
                }

                uploaded.Add(new { fileName, url = $"/uploads/{fileName}" });
            }

            return Ok(uploaded);
        }
    }
}
