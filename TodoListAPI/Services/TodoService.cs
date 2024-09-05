using Microsoft.Data.SqlClient;
using TodoListAPI.Models;
using TodoListAPI.Services.Interfaces;

namespace TodoListAPI.Services
{
    public class TodoService : ITodoService
    {
        private readonly string _connectionString;

        public TodoService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<TodoItem> GetAll()
        {
            var items = new List<TodoItem>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM TodoItem", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new TodoItem
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Description = reader.GetString(2),
                            IsCompleted = reader.GetBoolean(3)
                        });
                    }
                }
            }

            return items;
        }

        public TodoItem GetById(int id)
        {
            TodoItem item = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM TodoItem WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        item = new TodoItem
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Description = reader.GetString(2),
                            IsCompleted = reader.GetBoolean(3)
                        };
                    }
                }
            }

            return item;
        }

        public void Create(TodoItem item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    "INSERT INTO TodoItem (Title, Description, IsCompleted) " +
                    "OUTPUT INSERTED.Id " +
                    "VALUES (@Title, @Description, @IsCompleted)", connection);

                command.Parameters.AddWithValue("@Title", item.Title);
                command.Parameters.AddWithValue("@Description", item.Description);
                command.Parameters.AddWithValue("@IsCompleted", item.IsCompleted);

                // Retrieve the newly generated ID from the database
                item.Id = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void Update(int id, TodoItem item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE TodoItem SET Title = @Title, Description = @Description, IsCompleted = @IsCompleted WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Title", item.Title);
                command.Parameters.AddWithValue("@Description", item.Description);
                command.Parameters.AddWithValue("@IsCompleted", item.IsCompleted);
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM TodoItem WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }
    }
}
