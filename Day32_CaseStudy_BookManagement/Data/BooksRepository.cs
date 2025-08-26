// Here we will implement the methods for data access
// where users can search for books, add them to cart, and place
// orders. This app needs to interact with a SQL Server database to fetch book details, store order information, and manage
// customers

using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Day32_CaseStudy_BookManagement.Models;

namespace Day32_CaseStudy_BookManagement.Data
{
    public class BooksRepository
    {
        private readonly string _connectionString;

        public BooksRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            var books = new List<Book>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT * FROM Books", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var book = new Book
                            {
                                id = reader.GetInt32(0),
                                title = reader.GetString(1),
                                price = reader.GetDecimal(2)
                            };
                            books.Add(book);
                        }
                    }
                }
            }

            return books;
        }

        // Implement other methods for searching, adding to cart, and placing orders
        public async Task<Book> GetBookByIdAsync(int id)
        {
            Book book = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT * FROM Books WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            book = new Book
                            {
                                id = reader.GetInt32(0),
                                title = reader.GetString(1),
                                price = reader.GetDecimal(2)
                            };
                        }
                    }
                }
            }

            return book;
        }

        public async Task<List<Book>> SearchBooksAsync(string searchTerm)
        {
            var books = new List<Book>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT * FROM Books WHERE title LIKE @searchTerm", connection))
                {
                    command.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var book = new Book
                            {
                                id = reader.GetInt32(0),
                                title = reader.GetString(1),
                                price = reader.GetDecimal(2)
                            };
                            books.Add(book);
                        }
                    }
                }
            }

            return books;
        }

        // Create book
        public async Task AddBookAsync(Book book)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("INSERT INTO Books (id,title, price) VALUES (@id, @title, @price)", connection))
                {
                    command.Parameters.AddWithValue("@id", book.id);
                    command.Parameters.AddWithValue("@title", book.title);
                    command.Parameters.AddWithValue("@price", book.price);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        
    }
}
