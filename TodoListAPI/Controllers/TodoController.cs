using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoListAPI.Models;
using TodoListAPI.Services.Interfaces;

namespace TodoListAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        

        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        // GET: /api/todo
        [HttpGet]
        public IActionResult GetTodoItems()
        {
            var items = _todoService.GetAll();
            return Ok(items);
        }

        // GET: /api/todo/{id}
        [HttpGet("{id}")]
        public IActionResult GetTodoItem(int id)
        {
            var item = _todoService.GetById(id);
            if (item == null)
            {
                return NotFound(new { message = "TODO item not found." });
            }
            return Ok(item);
        }

        // POST: /api/todo
        [HttpPost]
        public IActionResult CreateTodoItem([FromBody] TodoItem newItem)
        {
            if (newItem == null)
            {
                return BadRequest(new { message = "Invalid TODO item." });
            }

            // Validation
            if (string.IsNullOrWhiteSpace(newItem.Title))
            {
                return BadRequest(new { message = "Title is required." });
            }

            _todoService.Create(newItem);
            return CreatedAtAction(nameof(GetTodoItem), new { id = newItem.Id }, newItem);
        }

        // PUT: /api/todo/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateTodoItem(int id, [FromBody] TodoItem updatedItem)
        {
            if (id != updatedItem.Id)
            {
                return BadRequest(new { message = "ID mismatch." });
            }

            var existingItem = _todoService.GetById(id);
            if (existingItem == null)
            {
                return NotFound(new { message = "TODO item not found." });
            }

            // Validation
            if (string.IsNullOrWhiteSpace(updatedItem.Title))
            {
                return BadRequest(new { message = "Title is required." });
            }

            _todoService.Update(id, updatedItem);
            return NoContent();
        }

        // DELETE: /api/todo/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteTodoItem(int id)
        {
            var item = _todoService.GetById(id);
            if (item == null)
            {
                return NotFound(new { message = "TODO item not found." });
            }

            _todoService.Delete(id);
            return NoContent();
        }



    }
}
