using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Demo_Exception_Handling
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" Steps for handling excpetion in C# using try, catch and finally blocks");
            // Exception exp = new Exception("This is an exception message"); // Exception is my base class for all types of exceptions

            //try
            //{
            //    //Taking two numbers from user to check division by zero exception
            //    Console.WriteLine("Enter first number: ");
            //    int num1 = Convert.ToInt32(Console.ReadLine());
            //    Console.WriteLine("Enter second number: ");
            //    int num2 = Convert.ToInt32(Console.ReadLine());
            //    int result = num1 / num2; // This line may throw a DivideByZeroException if num2 is zero

            //}
            //catch (Exception ex)// capable of catching all exception 
            //{
            //    Console.WriteLine($" Exception details " +ex.Message);

            //    throw;
            //}
            //finally
            //{
            //    // This block will always execute, regardless of whether an exception was thrown or not
            //    Console.WriteLine("Finally block executed. Cleanup can be done here.");
            //}



            //Step 1: We are creating an instance of OrderProcessor class
            OrderProcessor orderProcessor = new OrderProcessor();
            //Step 2: We are calling the PlaceOrder method with productId, Quantity and orderAmount
            orderProcessor.PlaceOrder("Smart phone", 2, 1000.00m);
            orderProcessor.PlaceOrder("Gaming PC", -1, 2000.00m);
            orderProcessor.PlaceOrder("Gaming PC", 2, 2000.00m);
            orderProcessor.PlaceOrder("Smart Watch", 1, 500.00m);
            Console.WriteLine();
        }
    }
}