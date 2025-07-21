using System;

namespace Day_3_jagged_array
{
    class Program
    {
        static void Main(string[] args)
        {
            // Declare a jagged array with 3 rows
            int[][] jagged = new int[3][];

            // Initialize each row with different lengths
            jagged[0] = new int[] { 10, 20 };
            jagged[1] = new int[] { 30, 40, 50 };
            jagged[2] = new int[] { 60 };

            // Print all elements
            Console.WriteLine("Jagged Array Elements:");
            for (int i = 0; i < jagged.Length; i++)
            {
                Console.Write("Row " + i + ": ");
                for (int j = 0; j < jagged[i].Length; j++)
                {
                    Console.Write(jagged[i][j] + " ");
                }
                Console.WriteLine(); // new line after each row
            }

            Console.ReadLine();
        }
    }
}
