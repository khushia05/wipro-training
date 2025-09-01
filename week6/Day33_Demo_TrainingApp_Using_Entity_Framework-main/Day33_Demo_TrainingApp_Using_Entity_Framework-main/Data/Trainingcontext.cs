using Microsoft.EntityFrameworkCore;
// using EF_DB_Connect_Demo.Models;
using TrainingApp.Models;
namespace TrainingApp.Data
{
    //Db context class is base class for entity framework
    public class TrainingContext : DbContext
    {
        public TrainingContext(DbContextOptions<TrainingContext> options) : base(options)
        {
        }
        public DbSet<Students> Students { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Course> Courses { get; set; }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     base.OnModelCreating(modelBuilder);
        //     // Configure relationships and constraints if needed
        //     modelBuilder.Entity<Course>()
        //         .HasOne(c => c.Trainer)
        //         .WithMany()
        //         .HasForeignKey(c => c.TrainerID);
        // }
    }
}