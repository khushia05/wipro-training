using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentAPlace.API.Data;

namespace RentAPlace.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyFeaturesController : ControllerBase
    {
        private readonly RentAPlaceDbContext _context;

        public PropertyFeaturesController(RentAPlaceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyFeatureDto>>> GetPropertyFeatures()
        {
            try
            {
                var propertyFeatures = await _context.PropertyFeatures
                    .Where(pf => pf.IsActive)
                    .Select(pf => new PropertyFeatureDto
                    {
                        Id = pf.Id,
                        Name = pf.Name,
                        Description = pf.Description
                    })
                    .ToListAsync();

                return Ok(propertyFeatures);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyFeatureDto>> GetPropertyFeature(int id)
        {
            try
            {
                var propertyFeature = await _context.PropertyFeatures
                    .Where(pf => pf.Id == id && pf.IsActive)
                    .Select(pf => new PropertyFeatureDto
                    {
                        Id = pf.Id,
                        Name = pf.Name,
                        Description = pf.Description
                    })
                    .FirstOrDefaultAsync();

                if (propertyFeature == null)
                {
                    return NotFound("Property feature not found");
                }

                return Ok(propertyFeature);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }

    public class PropertyFeatureDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
