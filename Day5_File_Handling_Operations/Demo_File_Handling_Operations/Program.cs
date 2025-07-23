using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading.Tasks;

namespace Demo_File_Handling_Operations
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the File Handling Operations Demo!");
            //step 1: Create a file, if not exists it will be created automatically
            //Step 2: Write to the file (it removed previous content and write new content)
            //Step 3: Read from the file
            //Step 4: Append to the file (Adding text to the end of file
            //Step 5: Delete the file

            //string filePath = "demoFile.txt";
            //// Step 1: Create a file
            //using (var fileStream = System.IO.File.Create(filePath))
            //{
            //    Console.WriteLine("File created successfully.");//Checking if file is created and opened successfully
            //}
            //// Step 2: Write to the file
            //File.WriteAllText(filePath, "Hello, this is a demo file.\n");// Writing text to the file, it will overwrite any existing content
            //                                                             // we can also create a file object after defiing mode ie reading, writing, appending

            //FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);//Filemode is used to specify how the file should be opened or created
            //// FileMode.Create will create a new file, and if the file already exists, it will be overwritten
            ///FileMode.Append will open the file if it exists and append data to the end of the file, or create a new file if it doesn't exist
            ///
            //// FileMode.OpenOrCreate will open the file if it exists or create a new one if it doesn't
            ////File stream for reading and writing in stream mode

            //Reading fom a file
            //string fileContent = File.ReadAllText(filePath);
            //Console.WriteLine("File content after writing:");
            //Console.WriteLine(fileContent);

            //// Step 4: Append to the file
            //File.AppendAllText(filePath, "This is appended text with time stamp : .\n "+DateTime.Now);// Appending text to the file
            //Console.WriteLine("File content after appending:");
            //fileContent = File.ReadAllText(filePath);
            //Console.WriteLine(fileContent);

            //// Step 5: Delete the file
            //if (File.Exists(filePath))// Checking if file exists before deleting
            //{
            //    File.Delete(filePath);// Deleting the file
            //    Console.WriteLine("File deleted successfully.");
            //}
            //else
            //{
            //    Console.WriteLine("File does not exist, cannot delete.");
            //}

            //loading data from file 

            //Employee Manager = new Employee(001,"BranchManager", 50000);


        }
    }
}