using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Repository.Interface
{
    public interface ITodoRepository
    {
        Task<List<Todo>> GetTodosAsync();
        Task<Todo?> GetTodoAsync(int id);
        Task<Todo> AddTodoAsync(Todo todo);
        Task<Todo?> UpdateTodoAsync(int id, Todo todo);
        Task<Todo> DeleteTodoAsync(int id);
    }
}