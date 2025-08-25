// See https://aka.ms/new-console-template for more information
using System.Data.SqlClient;
//using Microsoft.Data.SqlClient; // Use the correct namespace
//using Microsoft.Data.SqlTypes;
using Microsoft.Data.SqlClient;
using System;
using System.Data.SqlTypes;


class StudentDetail
{
    private readonly string connectionStr =
        "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CollegeDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

    public void InsertStudentDetail(int studentId,string firstName,string lastName)
    {
        string query = "INSERT INTO StudentDetail (StudentId, FirstName, LastName) VALUES (@StudentId, @FirstName, @LastName)";

        try
        {
            using (Microsoft.Data.SqlClient.SqlConnection sqlConnection = new Microsoft.Data.SqlClient.SqlConnection(connectionStr))
            using (Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(query, sqlConnection))
            {
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);

                sqlConnection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} row(s) inserted.");
                ViewData();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public void ViewData()
    {
        Console.WriteLine("Viewing all student records...");
        string query = "SELECT * FROM StudentDetail";

        try
        {
            using (Microsoft.Data.SqlClient.SqlConnection sqlConnection = new Microsoft.Data.SqlClient.SqlConnection(connectionStr))
            using (Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                using (Microsoft.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader())
                {   Console.WriteLine(); 
                    Console.WriteLine("StudentId  FirstName  LastName");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["StudentId"]}          {reader["FirstName"]}        {reader["LastName"]}");
                    }
                    Console.WriteLine();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }


    }

    public void DeleteStudentDetail(int id)
    {
        string query = "DELETE FROM StudentDetail WHERE StudentId = @StudentId";

        using (Microsoft.Data.SqlClient.SqlConnection sqlConnection = new Microsoft.Data.SqlClient.SqlConnection(connectionStr))
        using (Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(query, sqlConnection))
        {
            cmd.Parameters.AddWithValue("@StudentId", id);
            sqlConnection.Open();
           
         
            Console.WriteLine($"{cmd.ExecuteNonQuery()} row(s) deleted.");

            ViewData(); 
        }

        
    }

        public static void Main(string[] args)
        {
            StudentDetail studentDetail = new StudentDetail();
            while(true)
            {
                Console.WriteLine();
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Insert Student Detail");
                Console.WriteLine("2. View Student Details");
                Console.WriteLine("3. Delete Student Record");
                Console.WriteLine("4. Exit");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Inserting a new student record...");

                        Console.Write("Enter StudentId: ");
                        int studentId = Convert.ToInt32(Console.ReadLine());

                        Console.Write("Enter FirstName: ");
                        string firstName = Console.ReadLine();

                        Console.Write("Enter LastName: ");
                        string lastName = Console.ReadLine();

                   if(!string.IsNullOrEmpty(firstName) || !string.IsNullOrEmpty(lastName))
                    { 
                        Console.WriteLine("FirstName and LastName cannot be empty.");
                        break;
                    }

                        studentDetail.InsertStudentDetail(studentId,firstName,lastName);
                        break;

                    case "2":
                        studentDetail.ViewData();
                        break;
                    case "3":
                            Console.WriteLine("Enter Id To Delete ");
                            int Id= Convert.ToInt32(Console.ReadLine());

                            studentDetail.DeleteStudentDetail(Id);
                    break;

                case "4":
                    return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
        }
       
        }
}
