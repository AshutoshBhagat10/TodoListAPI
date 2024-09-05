using TodoListAPI.Models;

namespace TodoListAPI.Services.Interfaces
{
    public interface ITodoService
    {
        IEnumerable<TodoItem> GetAll();
        TodoItem GetById(int id);
        void Create(TodoItem item);
        void Update(int id, TodoItem item);
        void Delete(int id);

    }
}
