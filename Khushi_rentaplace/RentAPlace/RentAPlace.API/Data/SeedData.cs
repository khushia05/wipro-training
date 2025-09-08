using RentAPlace.API.Models;
using Microsoft.EntityFrameworkCore;

namespace RentAPlace.API.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var context = new RentAPlaceDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<RentAPlaceDbContext>>());

            // Check if we already have data
            if (context.Properties.Any())
            {
                return; // DB has been seeded
            }

            // Create sample owner
            var owner = new User
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "John",
                LastName = "Owner",
                Email = "owner@example.com",
                PasswordHash = "hashed_password_here",
                UserType = "Owner",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Users.Add(owner);

            // Get property types
            var apartmentType = context.PropertyTypes.First(pt => pt.Name == "Apartment");
            var houseType = context.PropertyTypes.First(pt => pt.Name == "House");
            var villaType = context.PropertyTypes.First(pt => pt.Name == "Villa");

            // Create sample properties
            var properties = new[]
            {
                new Property
                {
                    Name = "Luxury Apartment in Downtown",
                    Description = "Beautiful 2-bedroom apartment with modern amenities in the heart of the city.",
                    Address = "123 Main Street",
                    City = "New York",
                    State = "NY",
                    Country = "USA",
                    ZipCode = "10001",
                    Price = 150.00m,
                    PropertyTypeId = apartmentType.Id,
                    OwnerId = owner.Id,
                    Bedrooms = 2,
                    Bathrooms = 2,
                    Area = 1200,
                    MaxGuests = 4,
                    IsActive = true,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Property
                {
                    Name = "Cozy Brooklyn House",
                    Description = "Spacious family home with garden and modern kitchen.",
                    Address = "456 Oak Avenue",
                    City = "Brooklyn",
                    State = "NY",
                    Country = "USA",
                    ZipCode = "11201",
                    Price = 200.00m,
                    PropertyTypeId = houseType.Id,
                    OwnerId = owner.Id,
                    Bedrooms = 3,
                    Bathrooms = 2,
                    Area = 1800,
                    MaxGuests = 6,
                    IsActive = true,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Property
                {
                    Name = "Luxury Manhattan Villa",
                    Description = "Exclusive villa with premium amenities and stunning city views.",
                    Address = "789 Park Avenue",
                    City = "Manhattan",
                    State = "NY",
                    Country = "USA",
                    ZipCode = "10021",
                    Price = 300.00m,
                    PropertyTypeId = villaType.Id,
                    OwnerId = owner.Id,
                    Bedrooms = 4,
                    Bathrooms = 3,
                    Area = 2500,
                    MaxGuests = 8,
                    IsActive = true,
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            context.Properties.AddRange(properties);
            
            // Save properties first to generate IDs
            await context.SaveChangesAsync();

            // Add sample images for properties
            var images = new List<PropertyImage>();
            foreach (var property in properties)
            {
                for (int i = 1; i <= 5; i++)
                {
                    images.Add(new PropertyImage
                    {
                        PropertyId = property.Id,
                        ImageUrl = $"https://picsum.photos/800/600?random={property.Id + i}",
                        ImageName = $"property_{property.Id}_image_{i}.jpg",
                        IsPrimary = i == 1,
                        DisplayOrder = i,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            context.PropertyImages.AddRange(images);
            await context.SaveChangesAsync();
        }
    }
}
