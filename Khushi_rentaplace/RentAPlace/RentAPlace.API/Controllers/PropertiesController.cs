using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentAPlace.API.Data;
using RentAPlace.API.DTOs;
using System.Security.Claims;

namespace RentAPlace.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly RentAPlaceDbContext _context;

        public PropertiesController(RentAPlaceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyResponseDto>>> GetProperties(
            [FromQuery] string? search = null,
            [FromQuery] string? city = null,
            [FromQuery] string? state = null,
            [FromQuery] string? country = null,
            [FromQuery] string? propertyType = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] int? minBedrooms = null,
            [FromQuery] int? maxBedrooms = null,
            [FromQuery] int? minBathrooms = null,
            [FromQuery] int? maxBathrooms = null,
            [FromQuery] int? minGuests = null,
            [FromQuery] int? maxGuests = null,
            [FromQuery] bool? isAvailable = null,
            [FromQuery] string? category = null) // Keep for backward compatibility
        {
            try
            {
                var query = _context.Properties
                    .Include(p => p.PropertyType)
                    .Include(p => p.Owner)
                    .Include(p => p.Images)
                    .Where(p => p.IsActive);

                // Apply search filters
                if (!string.IsNullOrEmpty(search))
                {
                    var searchLower = search.ToLower();
                    query = query.Where(p => 
                        p.Name.ToLower().Contains(searchLower) ||
                        p.Description.ToLower().Contains(searchLower) ||
                        p.City.ToLower().Contains(searchLower) ||
                        p.State.ToLower().Contains(searchLower) ||
                        p.Address.ToLower().Contains(searchLower) ||
                        p.PropertyType.Name.ToLower().Contains(searchLower));
                }

                // Location filters
                if (!string.IsNullOrEmpty(city))
                {
                    query = query.Where(p => p.City.ToLower().Contains(city.ToLower()));
                }

                if (!string.IsNullOrEmpty(state))
                {
                    query = query.Where(p => p.State.ToLower().Contains(state.ToLower()));
                }

                if (!string.IsNullOrEmpty(country))
                {
                    query = query.Where(p => p.Country.ToLower().Contains(country.ToLower()));
                }

                // Property type filter (support both propertyType and category for backward compatibility)
                var typeFilter = propertyType ?? category;
                if (!string.IsNullOrEmpty(typeFilter))
                {
                    query = query.Where(p => p.PropertyType.Name.ToLower() == typeFilter.ToLower());
                }

                // Price range filters
                if (minPrice.HasValue)
                {
                    query = query.Where(p => p.Price >= minPrice.Value);
                }

                if (maxPrice.HasValue)
                {
                    query = query.Where(p => p.Price <= maxPrice.Value);
                }

                // Bedroom filters
                if (minBedrooms.HasValue)
                {
                    query = query.Where(p => p.Bedrooms >= minBedrooms.Value);
                }

                if (maxBedrooms.HasValue)
                {
                    query = query.Where(p => p.Bedrooms <= maxBedrooms.Value);
                }

                // Bathroom filters
                if (minBathrooms.HasValue)
                {
                    query = query.Where(p => p.Bathrooms >= minBathrooms.Value);
                }

                if (maxBathrooms.HasValue)
                {
                    query = query.Where(p => p.Bathrooms <= maxBathrooms.Value);
                }

                // Guest capacity filters
                if (minGuests.HasValue)
                {
                    query = query.Where(p => p.MaxGuests >= minGuests.Value);
                }

                if (maxGuests.HasValue)
                {
                    query = query.Where(p => p.MaxGuests <= maxGuests.Value);
                }

                // Availability filter
                if (isAvailable.HasValue)
                {
                    query = query.Where(p => p.IsAvailable == isAvailable.Value);
                }
                else
                {
                    // Default to available properties only if not specified
                    query = query.Where(p => p.IsAvailable);
                }

                var properties = await query
                    .Select(p => new PropertyResponseDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Address = p.Address,
                        City = p.City,
                        State = p.State,
                        ZipCode = p.ZipCode ?? "",
                        Price = p.Price,
                        PropertyType = p.PropertyType.Name,
                        Bedrooms = p.Bedrooms,
                        Bathrooms = p.Bathrooms,
                        Area = p.Area,
                        MaxGuests = p.MaxGuests,
                        Images = p.Images.Select(img => img.ImageUrl).ToList(),
                        Features = new List<string>(), // Simplified for now to avoid query issues
                        IsAvailable = p.IsAvailable,
                        Owner = new OwnerDto
                        {
                            Id = p.Owner.Id,
                            FirstName = p.Owner.FirstName,
                            LastName = p.Owner.LastName,
                            Email = p.Owner.Email
                        }
                    })
                    .OrderBy(p => p.Price) // Default ordering by price
                    .ToListAsync();

                return Ok(properties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyResponseDto>> GetProperty(int id)
        {
            try
            {
                var property = await _context.Properties
                    .Include(p => p.PropertyType)
                    .Include(p => p.Owner)
                    .Include(p => p.Images)
                    .Include(p => p.PropertyPropertyFeatures)
                        .ThenInclude(ppf => ppf.PropertyFeature)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (property == null)
                {
                    return NotFound("Property not found");
                }

                var propertyDto = new PropertyResponseDto
                {
                    Id = property.Id,
                    Name = property.Name,
                    Description = property.Description,
                    Address = property.Address,
                    City = property.City,
                    State = property.State,
                    ZipCode = property.ZipCode,
                    Price = property.Price,
                    PropertyType = property.PropertyType.Name,
                    Bedrooms = property.Bedrooms,
                    Bathrooms = property.Bathrooms,
                    Area = property.Area,
                    MaxGuests = property.MaxGuests,
                    Images = property.Images.Select(img => img.ImageUrl).ToList(),
                    Features = property.PropertyPropertyFeatures
                        .Select(ppf => ppf.PropertyFeature.Name)
                        .ToList(),
                    IsAvailable = property.IsAvailable,
                    Owner = new OwnerDto
                    {
                        Id = property.Owner.Id,
                        FirstName = property.Owner.FirstName,
                        LastName = property.Owner.LastName,
                        Email = property.Owner.Email
                    }
                };

                return Ok(propertyDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("owner")]
        public async Task<ActionResult<IEnumerable<PropertyResponseDto>>> GetOwnerProperties()
        {
            try
            {
                // For development: get all properties (since we don't have authentication yet)
                var properties = await _context.Properties
                    .Include(p => p.PropertyType)
                    .Include(p => p.Images)
                    .Where(p => p.IsActive)
                    .Select(p => new PropertyResponseDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Address = p.Address,
                        City = p.City,
                        State = p.State,
                        ZipCode = p.ZipCode ?? "",
                        Price = p.Price,
                        PropertyType = p.PropertyType.Name,
                        Bedrooms = p.Bedrooms,
                        Bathrooms = p.Bathrooms,
                        Area = p.Area,
                        MaxGuests = p.MaxGuests,
                        Images = p.Images.Select(img => img.ImageUrl).ToList(),
                        Features = new List<string>(), // Simplified for now
                        IsAvailable = p.IsAvailable,
                        TotalBookings = 0, // Simplified for now
                        TotalRevenue = 0 // Simplified for now
                    })
                    .ToListAsync();

                return Ok(properties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("owner/{ownerId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PropertyResponseDto>>> GetOwnerPropertiesById(string ownerId)
        {
            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId) || currentUserId != ownerId)
                {
                    return Forbid("You can only view your own properties");
                }

                var properties = await _context.Properties
                    .Include(p => p.PropertyType)
                    .Include(p => p.Images)
                    .Include(p => p.Bookings)
                    .Where(p => p.OwnerId == ownerId)
                    .Select(p => new PropertyResponseDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Address = p.Address,
                        City = p.City,
                        State = p.State,
                        ZipCode = p.ZipCode,
                        Price = p.Price,
                        PropertyType = p.PropertyType.Name,
                        Bedrooms = p.Bedrooms,
                        Bathrooms = p.Bathrooms,
                        Area = p.Area,
                        MaxGuests = p.MaxGuests,
                        Images = p.Images.Select(img => img.ImageUrl).ToList(),
                        Features = p.PropertyPropertyFeatures
                            .Select(ppf => ppf.PropertyFeature.Name)
                            .ToList(),
                        IsAvailable = p.IsAvailable,
                        TotalBookings = p.Bookings.Count(b => b.Status != Models.BookingStatus.Cancelled),
                        TotalRevenue = p.Bookings
                            .Where(b => b.Status != Models.BookingStatus.Cancelled)
                            .Sum(b => b.TotalPrice)
                    })
                    .ToListAsync();

                return Ok(properties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PropertyResponseDto>> CreateProperty([FromBody] CreatePropertyDto createDto)
        {
            try
            {
                // For now, we'll use a default owner ID since authentication might not be fully implemented
                var defaultOwnerId = await _context.Users.Select(u => u.Id).FirstOrDefaultAsync();
                if (string.IsNullOrEmpty(defaultOwnerId))
                {
                    return BadRequest("No users found in the system");
                }

                var property = new Models.Property
                {
                    Name = createDto.Name,
                    Description = createDto.Description,
                    Address = createDto.Address,
                    City = createDto.City,
                    State = createDto.State,
                    Country = createDto.Country,
                    ZipCode = createDto.ZipCode,
                    Price = createDto.Price,
                    PropertyTypeId = createDto.PropertyTypeId,
                    OwnerId = defaultOwnerId,
                    Bedrooms = createDto.Bedrooms,
                    Bathrooms = createDto.Bathrooms,
                    Area = createDto.Area,
                    MaxGuests = createDto.MaxGuests,
                    IsActive = true,
                    IsAvailable = createDto.IsAvailable,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Properties.Add(property);
                await _context.SaveChangesAsync();

                // Add property features if provided
                if (createDto.FeatureIds != null && createDto.FeatureIds.Any())
                {
                    var propertyFeatures = createDto.FeatureIds.Select(featureId => new Models.PropertyPropertyFeature
                    {
                        PropertyId = property.Id,
                        PropertyFeatureId = featureId
                    }).ToList();

                    _context.PropertyPropertyFeatures.AddRange(propertyFeatures);
                    await _context.SaveChangesAsync();
                }

        // Add property images if provided
        if (createDto.ImageUrls != null && createDto.ImageUrls.Any())
        {
            var propertyImages = createDto.ImageUrls.Select((imageUrl, index) => new Models.PropertyImage
            {
                PropertyId = property.Id,
                ImageUrl = imageUrl,
                ImageName = Path.GetFileName(imageUrl) ?? $"property_{property.Id}_image_{index + 1}.jpg",
                IsPrimary = index == 0, // First image is primary
                DisplayOrder = index + 1,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            _context.PropertyImages.AddRange(propertyImages);
            await _context.SaveChangesAsync();
        }

                // Load the created property with all related data
                var createdProperty = await _context.Properties
                    .Include(p => p.PropertyType)
                    .Include(p => p.Owner)
                    .Include(p => p.Images)
                    .Include(p => p.PropertyPropertyFeatures)
                        .ThenInclude(ppf => ppf.PropertyFeature)
                    .FirstAsync(p => p.Id == property.Id);

                var responseDto = new PropertyResponseDto
                {
                    Id = createdProperty.Id,
                    Name = createdProperty.Name,
                    Description = createdProperty.Description,
                    Address = createdProperty.Address,
                    City = createdProperty.City,
                    State = createdProperty.State,
                    ZipCode = createdProperty.ZipCode ?? "",
                    Price = createdProperty.Price,
                    PropertyType = createdProperty.PropertyType.Name,
                    Bedrooms = createdProperty.Bedrooms,
                    Bathrooms = createdProperty.Bathrooms,
                    Area = createdProperty.Area,
                    MaxGuests = createdProperty.MaxGuests,
                    Images = createdProperty.Images.Select(img => img.ImageUrl).ToList(),
                    Features = createdProperty.PropertyPropertyFeatures
                        .Select(ppf => ppf.PropertyFeature.Name)
                        .ToList(),
                    IsAvailable = createdProperty.IsAvailable,
                    Owner = new OwnerDto
                    {
                        Id = createdProperty.Owner.Id,
                        FirstName = createdProperty.Owner.FirstName,
                        LastName = createdProperty.Owner.LastName,
                        Email = createdProperty.Owner.Email
                    }
                };

                return CreatedAtAction(nameof(GetProperty), new { id = property.Id }, responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }

    public class CreatePropertyDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string? ZipCode { get; set; }
        public decimal Price { get; set; }
        public int PropertyTypeId { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int Area { get; set; }
        public int MaxGuests { get; set; }
        public bool IsAvailable { get; set; } = true;
        public List<int>? FeatureIds { get; set; }
        public List<string>? ImageUrls { get; set; }
    }

    public class PropertyResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string PropertyType { get; set; } = string.Empty;
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int Area { get; set; }
        public int MaxGuests { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public List<string> Features { get; set; } = new List<string>();
        public bool IsAvailable { get; set; }
        public OwnerDto? Owner { get; set; }
        public int TotalBookings { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class OwnerDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
