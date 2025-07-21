using System;

namespace demo_student_grade_system
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Student Grade System");
            Console.Write("Enter the marks of student (0-100): ");

            int marks;
            bool isValid = int.TryParse(Console.ReadLine(), out marks);

            if (!isValid || marks < 0 || marks > 100)
            {
                Console.WriteLine("Invalid marks. Please enter a number between 0 and 100.");
            }
            else
            {
                string grade;

                if (marks >= 90)
                    grade = "A+";
                else if (marks >= 80)
                    grade = "A";
                else if (marks >= 70)
                    grade = "B";
                else if (marks >= 60)
                    grade = "C";
                else if (marks >= 50)
                    grade = "D";
                else
                    grade = "Fail";

                Console.WriteLine("Grade: " + grade);
            }

            Console.ReadLine();
        }
    }
}
