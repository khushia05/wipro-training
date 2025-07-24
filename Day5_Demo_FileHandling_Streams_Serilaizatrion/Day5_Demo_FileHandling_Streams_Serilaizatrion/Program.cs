using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;// System.Text.Json is used for serialization and deserialization of objects to and from JSON format
using System.IO;// System.IO is used for file handling in C#
//using System.Memory;
//using System.Threading.Tasks.Extensions;// This namespace is used for asynchronous programming in C#

namespace Demo_Filehandling_Streams
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public Employee(int id, string name, string department)
        {
            Id = id;
            Name = name;
            Department = department;
        }

    }
    internal class Program
    {
        static void Main(string[] args)
        {
            // In file handling we have File streams which a implemented via Stream
            //Serialization which is implemented via System.Text.Json
            //Deserialization which is implemented via System.Text.Json
            //Why: It is used whenever we want to read or write data to a file in a structured way, such as reading a text file, writing to a binary file, or

            //Folllowing are trhe steps to handle files in C#: 
            //Step 1: Create a file stream using FileStream class ex FileStream fileStream = new FileStream("example.txt", FileMode.Create);
            //Step 2: Use the stream to read or write data using StreamReader or StreamWriter classes.
            //Step 3: Close the file stream to release resources.( It is very imp to relase resources after using file streams to avoid memory leaks and file locks.)
            //Step 4: Use try-catch blocks to handle exceptions that may occur during file operations, such as file not found or access denied.

            //Step 1: Creating a list of employees
            List<Employee> employees = new List<Employee>
            {
                new Employee(1, "John Doe", "HR"),
                new Employee(2, "Jane Smith", "IT"),
                new Employee(3, "Sam Brown", "Finance")
            };

            //Serializing the list of employees to a JSON string
            // string jsonString = System.Text.Json.JsonSerializer.Serialize(employees);// Serializing the list of employees to a JSON string
            //by serailizing we are converting the object to a string format which can be stored in a file or sent over the network.

            //defing the file path where the JSON string will be written
            //string filePath = Path.Combine(Directory.GetCurrentDirectory(), "employees.json");

            //Step 2: Defining the file path where the JSON string will be written
            string filePath = @"C:\Users\Parth\source\repos\Wipro .NET with react cohort\Week 1 C# Demo\Day 5 Demo Delegates, Events Collections and File IO\Demo_Filehandling_Streams\bin\Debug\employees.json"; // Defining the file path where the JSON string will be written
            // This will create a file path in the current directory with the name "employees.json"

            //Writing the JSON string to a file
            using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate))// Creating a file stream to write the JSON string to a file named "employees.json"
            {
                //using (StreamWriter writer = new StreamWriter(fileStream))
                // using (FileStream fs = new FileStream(filePath, FileMode.Create)) // Using FileStream to create or overwrite the file
                {
                    //writer.Write(employees);//this writes the JSON string to the file
                    //instead of write we can use
                    JsonSerializer.Serialize(fileStream, employees); // this will serialize the object to the file directly
                    Console.WriteLine("Employees data written to the file successfully ..!!!");
                }
            }

            //Reading the JSON string from the file
            // List<Employee> LoadedEmployees;
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open)) // Creating a file stream to read the JSON string from the file
            {
                //using (StreamReader reader = new StreamReader(fileStream))
                {
                    //string jsonString = reader.ReadToEnd(); // Reading the JSON string from the file
                    // LoadedEmployees = JsonSerializer.Deserialize<List<Employee>>(fileStream); // this will deserialize the JSON string to a list of employees
                    List<Employee> LoadedEmployees = JsonSerializer.Deserialize<List<Employee>>(fileStream); // this will deserialize the JSON string to a list of employees
                    Console.WriteLine("Employees data read from the file successfully ..!!!");

                    //Displaying the loaded employees data
                    Console.WriteLine("\n Loaded Employees from the file : ");
                    foreach (var emp in LoadedEmployees)
                    {
                        Console.WriteLine($"Id: {emp.Id}, Name: {emp.Name}, Department: {emp.Department}");
                    }
                }
            }



        }
    }
}