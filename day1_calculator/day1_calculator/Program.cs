using System;

namespace SimpleCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Simple Calculator");

            // Input first number
            Console.Write("Enter first number: ");
            double num1 = Convert.ToDouble(Console.ReadLine());

            // Input operator
            Console.Write("Enter operator (+, -, *, /): ");
            string op = Console.ReadLine();

            // Input second number
            Console.Write("Enter second number: ");
            double num2 = Convert.ToDouble(Console.ReadLine());

            double result = 0;
            bool validOperation = true;

            // Perform calculation based on operator
            switch (op)
            {
                case "+":
                    result = num1 + num2;
                    break;

                case "-":
                    result = num1 - num2;
                    break;

                case "*":
                    result = num1 * num2;
                    break;

                case "/":
                    if (num2 != 0)
                        result = num1 / num2;
                    else
                    {
                        Console.WriteLine("Error: Cannot divide by zero.");
                        validOperation = false;
                    }
                    break;

                default:
                    Console.WriteLine("Invalid operator.");
                    validOperation = false;
                    break;
            }

            // Display result
            if (validOperation)
            {
                Console.WriteLine("Result: " + result);
            }

            Console.ReadLine();
        }
    }
}
