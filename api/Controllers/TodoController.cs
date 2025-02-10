using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Todo;
using api.Services;
using Microsoft.AspNetCore.Mvc;
namespace api.Controllers
{
    [ApiController]
    [Route("v1/todos")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        // Get all todos
        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            var todos = await _todoService.GetTodosAsync();
            return Ok(todos);
        }

        // Get a single todo by id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTodo([FromRoute] int id)
        {
            var todo = await _todoService.GetTodoAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            return Ok(todo);
        }

        // Add a new todo
        [HttpPost]
        public async Task<IActionResult> AddTodo([FromBody] TodoRequest todoRequest)
        {
            if (todoRequest == null)
            {
                return BadRequest("Invalid data.");
            }

            var todoDto = await _todoService.AddTodoAsync(todoRequest);
            return Ok(todoDto);
        }

        // Update an existing todo
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTodo([FromRoute] int id, [FromBody] TodoRequest todoRequest)
        {
            if (todoRequest == null)
            {
                return BadRequest("Invalid data.");
            }

            var updatedTodo = await _todoService.UpdateTodoAsync(id, todoRequest);
            if (updatedTodo == null)
            {
                return NotFound();
            }
            return Ok(updatedTodo);
        }

        // Delete a todo
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTodo([FromRoute] int id)
        {
            var deletedTodo = await _todoService.DeleteTodoAsync(id);
            if (deletedTodo == null)
            {
                return NotFound();
            }
            return Ok(deletedTodo);
        }
    }
}