using System;
using System.Collections.Generic;

namespace Demo_Stack_Queue_Generic_Collection
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Stack and Queue using Generic Collection in C#\n");

            // Stack for browser navigation (LIFO)
            Stack<string> browserHistory = new Stack<string>();
            browserHistory.Push("https://google.com");
            browserHistory.Push("https://youtube.com");
            browserHistory.Push("https://github.com");
            browserHistory.Push("https://openai.com");

            Console.WriteLine("Browser Navigation History (Stack - LIFO):");
            foreach (var url in browserHistory)
            {
                Console.WriteLine(url);
            }

            // Simulate "Back" button press
            Console.WriteLine("\nGoing back from: " + browserHistory.Pop());
            Console.WriteLine("Current page: " + browserHistory.Peek());

            Console.WriteLine("\nUpdated Browser History:");
            foreach (var url in browserHistory)
            {
                Console.WriteLine(url);
            }

            Console.WriteLine("\n------------------------------\n");

            // Queue for print job queue (FIFO)
            Queue<string> printJobs = new Queue<string>();
            printJobs.Enqueue("Print job 1: Resume.pdf");
            printJobs.Enqueue("Print job 2: Invoice.docx");
            printJobs.Enqueue("Print job 3: Poster.png");

            Console.WriteLine("Print Job Queue (Queue - FIFO):");
            foreach (var job in printJobs)
            {
                Console.WriteLine(job);
            }

            // Simulate processing a print job
            Console.WriteLine("\nProcessing: " + printJobs.Dequeue());
            Console.WriteLine("Next job: " + printJobs.Peek());

            Console.WriteLine("\nUpdated Print Job Queue:");
            foreach (var job in printJobs)
            {
                Console.WriteLine(job);
            }

            Console.ReadLine();
        }
    }
}
