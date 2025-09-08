using Microsoft.EntityFrameworkCore;
using RentAPlace.API.Models;

namespace RentAPlace.API.Data
{
    public class RentAPlaceDbContext : DbContext
    {
        public RentAPlaceDbContext(DbContextOptions<RentAPlaceDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<PropertyFeature> PropertyFeatures { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<PropertyPropertyFeature> PropertyPropertyFeatures { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.UserType);
                entity.Property(e => e.UserType).HasConversion<string>();
            });

            // Configure Property entity
            modelBuilder.Entity<Property>(entity =>
            {
                entity.HasIndex(e => e.OwnerId);
                entity.HasIndex(e => e.City);
                entity.HasIndex(e => e.PropertyTypeId);
                entity.HasIndex(e => e.IsActive);
                entity.HasIndex(e => e.IsAvailable);
                
                entity.HasOne(p => p.Owner)
                    .WithMany(u => u.Properties)
                    .HasForeignKey(p => p.OwnerId)
                    .OnDelete(DeleteBehavior.NoAction);
                    
                entity.HasOne(p => p.PropertyType)
                    .WithMany()
                    .HasForeignKey(p => p.PropertyTypeId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Configure PropertyImage entity
            modelBuilder.Entity<PropertyImage>(entity =>
            {
                entity.HasIndex(e => e.PropertyId);
                
                entity.HasOne(pi => pi.Property)
                    .WithMany(p => p.Images)
                    .HasForeignKey(pi => pi.PropertyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Reservation entity
            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasIndex(e => e.PropertyId);
                entity.HasIndex(e => e.RenterId);
                entity.HasIndex(e => e.Status);
                entity.Property(e => e.Status).HasConversion<string>();
                
                entity.HasOne(r => r.Property)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(r => r.PropertyId)
                    .OnDelete(DeleteBehavior.NoAction);
                    
                entity.HasOne(r => r.Renter)
                    .WithMany(u => u.Reservations)
                    .HasForeignKey(r => r.RenterId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Configure Message entity
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasIndex(e => e.PropertyId);
                entity.HasIndex(e => e.SenderId);
                entity.HasIndex(e => e.ReceiverId);
            });

            // Configure Review entity
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasIndex(e => e.PropertyId);
                entity.HasIndex(e => e.RenterId);
                entity.HasIndex(e => new { e.PropertyId, e.RenterId, e.ReservationId }).IsUnique();
                
                entity.HasOne(r => r.Property)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(r => r.PropertyId)
                    .OnDelete(DeleteBehavior.NoAction);
                    
                entity.HasOne(r => r.Renter)
                    .WithMany(u => u.Reviews)
                    .HasForeignKey(r => r.RenterId)
                    .OnDelete(DeleteBehavior.NoAction);
                    
                entity.HasOne(r => r.Reservation)
                    .WithMany(res => res.Reviews)
                    .HasForeignKey(r => r.ReservationId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Configure Notification entity
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.RecipientEmail);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.Type);
                entity.HasIndex(e => e.CreatedAt);
                entity.HasIndex(e => e.NextRetryAt);
                
                entity.Property(e => e.RecipientEmail)
                    .IsRequired()
                    .HasMaxLength(450);
                    
                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(200);
                    
                entity.Property(e => e.Body)
                    .IsRequired();
                    
                entity.Property(e => e.Type)
                    .HasMaxLength(50);
                    
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValue("Pending");
                    
                entity.Property(e => e.ErrorMessage)
                    .HasMaxLength(1000);
                    
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // Configure many-to-many relationship between Property and PropertyFeature
            modelBuilder.Entity<PropertyPropertyFeature>(entity =>
            {
                entity.HasIndex(e => new { e.PropertyId, e.PropertyFeatureId }).IsUnique();
                
                entity.HasOne(ppf => ppf.Property)
                    .WithMany(p => p.PropertyPropertyFeatures)
                    .HasForeignKey(ppf => ppf.PropertyId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(ppf => ppf.PropertyFeature)
                    .WithMany()
                    .HasForeignKey(ppf => ppf.PropertyFeatureId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Message relationships
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Property)
                .WithMany(p => p.Messages)
                .HasForeignKey(m => m.PropertyId)
                .OnDelete(DeleteBehavior.NoAction);

            // Seed data for PropertyTypes
            modelBuilder.Entity<PropertyType>().HasData(
                new PropertyType { Id = 1, Name = "Apartment", Description = "A self-contained housing unit in a larger building" },
                new PropertyType { Id = 2, Name = "House", Description = "A single-family dwelling" },
                new PropertyType { Id = 3, Name = "Villa", Description = "A large, luxurious house, often in a rural or suburban setting" },
                new PropertyType { Id = 4, Name = "Condo", Description = "A privately owned individual unit in a multi-unit building" },
                new PropertyType { Id = 5, Name = "Studio", Description = "A small apartment with a combined living and sleeping area" },
                new PropertyType { Id = 6, Name = "Loft", Description = "A large, open space converted from industrial use" },
                new PropertyType { Id = 7, Name = "Townhouse", Description = "A multi-story house sharing walls with adjacent properties" }
            );

            // Seed data for PropertyFeatures
            modelBuilder.Entity<PropertyFeature>().HasData(
                new PropertyFeature { Id = 1, Name = "WiFi", Description = "High-speed internet access" },
                new PropertyFeature { Id = 2, Name = "Pool", Description = "Swimming pool available" },
                new PropertyFeature { Id = 3, Name = "Garden", Description = "Outdoor garden space" },
                new PropertyFeature { Id = 4, Name = "Beach Access", Description = "Direct access to beach" },
                new PropertyFeature { Id = 5, Name = "Parking", Description = "Parking space available" },
                new PropertyFeature { Id = 6, Name = "Kitchen", Description = "Fully equipped kitchen" },
                new PropertyFeature { Id = 7, Name = "Air Conditioning", Description = "Air conditioning system" },
                new PropertyFeature { Id = 8, Name = "Heating", Description = "Heating system" },
                new PropertyFeature { Id = 9, Name = "Pet Friendly", Description = "Pets are allowed" },
                new PropertyFeature { Id = 10, Name = "Smoking Allowed", Description = "Smoking is permitted" },
                new PropertyFeature { Id = 11, Name = "Balcony", Description = "Private balcony or terrace" },
                new PropertyFeature { Id = 12, Name = "Fireplace", Description = "Fireplace available" },
                new PropertyFeature { Id = 13, Name = "Gym", Description = "Fitness center or gym" },
                new PropertyFeature { Id = 14, Name = "Spa", Description = "Spa or wellness facilities" },
                new PropertyFeature { Id = 15, Name = "Ocean View", Description = "View of the ocean" },
                new PropertyFeature { Id = 16, Name = "Mountain View", Description = "View of mountains" },
                new PropertyFeature { Id = 17, Name = "City View", Description = "View of the city" },
                new PropertyFeature { Id = 18, Name = "Lake View", Description = "View of a lake" },
                new PropertyFeature { Id = 19, Name = "Rooftop", Description = "Rooftop access or terrace" },
                new PropertyFeature { Id = 20, Name = "Hot Tub", Description = "Hot tub or jacuzzi available" }
            );

            // Configure Booking entity
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(10,2)");
                entity.Property(e => e.CheckInDate).HasColumnType("date");
                entity.Property(e => e.CheckOutDate).HasColumnType("date");
                entity.Property(e => e.CreatedAt).HasColumnType("datetime2");
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime2");

                entity.HasOne(e => e.Property)
                    .WithMany()
                    .HasForeignKey(e => e.PropertyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
