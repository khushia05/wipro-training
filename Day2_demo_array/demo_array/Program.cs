using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_array
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Step 1: Declare a 2D array for seating arrangement with rows and columns
            //Step 2: Initialize the 2D array with seat numbers and availability status
            //Step 3: Display the seating arrangement after initialization and before booking
            //Step 4: Allow users to book seats by selecting a seat number
            //Step 5: Display the seating arrangement after booking

                // Step 1: Declare a 2D array for seating arrangement (e.g., 5 rows and 4 columns)
                string[,] seats = new string[5, 4];

                // Step 2: Initialize the 2D array with seat numbers and availability status
                int seatNumber = 1;
                for (int i = 0; i < 5; i++)  // Rows
                {
                    for (int j = 0; j < 4; j++) // Columns
                    {
                        seats[i, j] = "S" + seatNumber.ToString("00");  // Example: S01, S02, ...
                        seatNumber++;
                    }
                }

                // Step 3: Display the seating arrangement before booking
                Console.WriteLine("Initial Seating Arrangement:");
                DisplaySeats(seats);

                // Step 4: Allow users to book seats
                Console.Write("\nEnter the seat number to book (e.g., S05): ");
                string seatToBook = Console.ReadLine();

                bool isBooked = false;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (seats[i, j] == seatToBook)
                        {
                            seats[i, j] = "X";  // X means booked
                            isBooked = true;
                            break;
                        }
                    }
                }

                if (isBooked)
                    Console.WriteLine($"\nSeat {seatToBook} booked successfully!");
                else
                    Console.WriteLine($"\nSeat {seatToBook} not found or already booked!");

                // Step 5: Display the seating arrangement after booking
                Console.WriteLine("\nUpdated Seating Arrangement:");
                DisplaySeats(seats);

                Console.ReadLine();
            }

            static void DisplaySeats(string[,] seats)
            {
                for (int i = 0; i < seats.GetLength(0); i++)
                {
                    for (int j = 0; j < seats.GetLength(1); j++)
                    {
                        Console.Write(seats[i, j] + "\t");
                    }
                    Console.WriteLine();
                }
            }
        }
    }

