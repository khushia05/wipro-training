using Microsoft.EntityFrameworkCore;
using TrainingApp.Models;
using TrainingApp.Data;
var optionsBuilder = new DbContextOptionsBuilder<TrainingContext>();
optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TestDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
using var context = new TrainingContext(optionsBuilder.Options);


if (!context.Students.Any())
{
    var trainer = new Trainer { Name = "Salman khan" };
    var student = new Students { Name = "Student1" };
    var course = new Course { Title = "Driving", Trainer = trainer };
    context.Trainers.Add(trainer);
    context.Students.Add(student);
    context.Courses.Add(course);
    context.SaveChanges();
    Console.WriteLine("Sample data inserted!");
}
else
{
    Console.WriteLine("Database already has data.");
}