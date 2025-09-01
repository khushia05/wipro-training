using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Design;
using TrainingApp.Data;
// using TrainingApp.Data;
namespace TrainingApp.Data
{
    public class DesignTimeDbContext : IDesignTimeDbContextFactory<TrainingContext>
    {
        public TrainingContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TrainingContext>();
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TestDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            return new TrainingContext(optionsBuilder.Options);
        }
    }
}