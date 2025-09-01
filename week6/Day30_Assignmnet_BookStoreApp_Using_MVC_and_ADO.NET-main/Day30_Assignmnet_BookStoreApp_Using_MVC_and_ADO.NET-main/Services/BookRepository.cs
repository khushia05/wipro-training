using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BookStoreApp.Models;
using YourApp.Models;
using Microsoft.Data.SqlClient;

namespace BookStoreApp.Services
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetAllBooks();                // SqlDataReader
        Book GetBookById(int id);                       // SqlCommand + parameter
        int AddBook(Book book);                         // Stored proc
        void UpdateBook(Book book);                     // Stored proc
        void DeleteBook(int id);                        // Stored proc
        DataSet GetBooksDataSet();                      // Disconnected: DataSet
        void SaveDataSetChanges(DataSet ds);            // Push changes via SqlDataAdapter
    }

    public class BookRepository : IBookRepository
    {
        private readonly string? _connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BookStoreDb;Integrated Security=True;TrustServerCertificate=True;";

        public BookRepository(IConfiguration config)
        {
            // _connString = config.GetConnectionString("DefaultConnection");
        }

        // Connected: use SqlDataReader for fast forward-only reads
        public IEnumerable<Book> GetAllBooks()
        {
            var list = new List<Book>();
            using (var conn = new SqlConnection(_connString))
            using (var cmd = new SqlCommand("SELECT BookId, Title, Author, ISBN, Price, PublishedDate, Description FROM Books ORDER BY Title", conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Book
                        {
                            BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Author = reader.GetString(reader.GetOrdinal("Author")),
                            ISBN = reader.IsDBNull(reader.GetOrdinal("ISBN")) ? null : reader.GetString(reader.GetOrdinal("ISBN")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            PublishedDate = reader.IsDBNull(reader.GetOrdinal("PublishedDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("PublishedDate")),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description"))
                        });
                    }
                }
            }
            return list;
        }

        public Book GetBookById(int id)
        {
            using (var conn = new SqlConnection(_connString))
            using (var cmd = new SqlCommand("SELECT BookId, Title, Author, ISBN, Price, PublishedDate, Description FROM Books WHERE BookId = @BookId", conn))
            {
                cmd.Parameters.Add("@BookId", SqlDbType.Int).Value = id;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Book
                        {
                            BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Author = reader.GetString(reader.GetOrdinal("Author")),
                            ISBN = reader.IsDBNull(reader.GetOrdinal("ISBN")) ? null : reader.GetString(reader.GetOrdinal("ISBN")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            PublishedDate = reader.IsDBNull(reader.GetOrdinal("PublishedDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("PublishedDate")),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description"))
                        };
                    }
                }
            }
            return null;
        }

        // Stored procedure: Add book, returns new id
        public int AddBook(Book book)
        {
            using (var conn = new SqlConnection(_connString))
            using (var cmd = new SqlCommand("sp_AddBook", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@Author", book.Author);
                cmd.Parameters.AddWithValue("@ISBN", (object)book.ISBN ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Price", book.Price);
                cmd.Parameters.AddWithValue("@PublishedDate", (object)book.PublishedDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Description", (object)book.Description ?? DBNull.Value);

                var outParam = new SqlParameter("@NewBookId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outParam);

                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)outParam.Value;
            }
        }

        // Stored procedure: Update
        public void UpdateBook(Book book)
        {
            using (var conn = new SqlConnection(_connString))
            using (var cmd = new SqlCommand("sp_UpdateBook", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BookId", book.BookId);
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@Author", book.Author);
                cmd.Parameters.AddWithValue("@ISBN", (object)book.ISBN ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Price", book.Price);
                cmd.Parameters.AddWithValue("@PublishedDate", (object)book.PublishedDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Description", (object)book.Description ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Stored procedure: Delete
        public void DeleteBook(int id)
        {
            using (var conn = new SqlConnection(_connString))
            using (var cmd = new SqlCommand("sp_DeleteBook", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BookId", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Disconnected: Get DataSet (SqlDataAdapter)
        public DataSet GetBooksDataSet()
        {
            var ds = new DataSet();
            using (var conn = new SqlConnection(_connString))
            using (var da = new SqlDataAdapter("SELECT BookId, Title, Author, ISBN, Price, PublishedDate, Description FROM Books", conn))
            {
                // For update/insert/delete with DataAdapter, we need commands; create a command builder or explicit commands.
                var cb = new SqlCommandBuilder(da); // simple approach: generates insert/update/delete commands automatically
                da.Fill(ds, "Books");
            }
            return ds;
        }

        // Push changes from DataSet back to DB
        public void SaveDataSetChanges(DataSet ds)
        {
            if (ds == null || !ds.Tables.Contains("Books")) return;
            using (var conn = new SqlConnection(_connString))
            using (var da = new SqlDataAdapter("SELECT BookId, Title, Author, ISBN, Price, PublishedDate, Description FROM Books", conn))
            {
                var cb = new SqlCommandBuilder(da);
                conn.Open();
                da.Update(ds, "Books");
            }
        }
    }
}
