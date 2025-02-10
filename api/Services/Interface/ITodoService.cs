using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Todo;

namespace api.Services
{
    public interface ITodoService
    {
        Task<List<TodoDto>> GetTodosAsync();
        Task<TodoDto?> GetTodoAsync(int id);
        Task<TodoDto> AddTodoAsync(TodoRequest todo);
        Task<TodoDto?> UpdateTodoAsync(int id, TodoRequest todo);
        Task<TodoDto> DeleteTodoAsync(int id);
    }
}
