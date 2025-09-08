using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentAPlace.API.Data;

namespace RentAPlace.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyTypesController : ControllerBase
    {
        private readonly RentAPlaceDbContext _context;

        public PropertyTypesController(RentAPlaceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyTypeDto>>> GetPropertyTypes()
        {
            try
            {
                var propertyTypes = await _context.PropertyTypes
                    .Where(pt => pt.IsActive)
                    .Select(pt => new PropertyTypeDto
                    {
                        Id = pt.Id,
                        Name = pt.Name,
                        Description = pt.Description
                    })
                    .ToListAsync();

                return Ok(propertyTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyTypeDto>> GetPropertyType(int id)
        {
            try
            {
                var propertyType = await _context.PropertyTypes
                    .Where(pt => pt.Id == id && pt.IsActive)
                    .Select(pt => new PropertyTypeDto
                    {
                        Id = pt.Id,
                        Name = pt.Name,
                        Description = pt.Description
                    })
                    .FirstOrDefaultAsync();

                if (propertyType == null)
                {
                    return NotFound("Property type not found");
                }

                return Ok(propertyType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }

    public class PropertyTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
