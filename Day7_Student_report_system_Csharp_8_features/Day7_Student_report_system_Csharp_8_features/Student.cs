using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEmo_Student_report_system_Csharp_8_features
{
    public interface IReport // Here we are defining default interface method
    {
        void GenerateReport(List<Student> students);

        void PrintTitle() => Console.WriteLine("\n Student report...!!\n");
    }

    public class Student
    {
        public int Id { get; }//read only members 
        public string Name { get; set; }
        public string? Email { get; set; }// nullable reference types
        public int Marks { get; set; }

        public Student(int id, string name, int marks)
        {
            Id = id;
            Name = name;
            Marks = marks;
        }
    }
    public class ReportGenerator : IReport
    {
        public async IAsyncEnumerable<Student> LoadStudntAsync() //Here async is used for Asynchronous communication and IAsyncEnumnerator is used for enumerating type of collection values
        {
            //IEnumerable<student> is an interface in C# that repreesent a sequence of Studen objects that can be enumaerated (Iteraed over)
            var students = new List<Student>
            {
                new Student(1, "Rajat", 85),
                new Student(2, "Nidhi", 86),
                new Student(3, "Sreekant", 89),
                new Student(4, "Raghav", 90)
            };
            foreach (var student in students)
            {
                await Task.Delay(300); // Simulating data fetch
                yield return student; // yield is used fto implement iterator methoid to return elelemen one at a time 
            }
        }
        public async void GenerateReport(List<Student> students)
        {
            PrintTitle();

            foreach (var student in students)
            {
                string grade = student.Marks switch
                {
                    >= 90 => "A",
                    >= 75 => "B",
                    >= 60 => "C",
                    _ => "D"
                };
                Console.WriteLine($"{student.Name,-10}| Marks:{student.Marks,-3} | Grade: {grade}");//-10 and -3 are nof of char we want to display 
            }
        }
    }

    public static class ReportUtils // implmemting range and indices to get top 3 students 
    {
        public static List<Student> GeTopPerformers(List<Student> students)
        {
            var sorted = students.OrderByDescending(s => s.Marks).ToList();
            return sorted[..3]; // using range to slice top 3 values 
        }
    }

    //Creating a class for exporting report 
    public static class ExportHelper
    {
        public static void ExporReport(List<Student> students)
        {
            using var writer = new StreamWriter("StudentReport.txt");
            writer.WriteLine("Name\t Marks");
            foreach (var s in students)
            {
                writer.WriteLine($"{s.Name} \t{s.Marks}");
            }

            Console.WriteLine("\n report exported to StudentReport.txt");
        }
    }
}