using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Todo;
using api.Mappers;
using api.Repository.Interface;
namespace api.Services
{
    public class TodoService(ITodoRepository todoRepository) : ITodoService
    {
        private readonly ITodoRepository _todoRepository = todoRepository;

        public async Task<TodoDto> AddTodoAsync(TodoRequest todoRequest)
        {
            var todoEntity = TodoMapper.ToEntity(todoRequest);
            var addedTodo = await _todoRepository.AddTodoAsync(todoEntity);
            return TodoMapper.ToDto(addedTodo);
        }

        public async Task<TodoDto> DeleteTodoAsync(int id)
        {
            var deletedTodo = await _todoRepository.DeleteTodoAsync(id);
            return TodoMapper.ToDto(deletedTodo);
        }

        public async Task<TodoDto?> GetTodoAsync(int id)
        {
            var todo = await _todoRepository.GetTodoAsync(id);
            return todo == null ? null : TodoMapper.ToDto(todo);
        }

        public async Task<List<TodoDto>> GetTodosAsync()
        {
            var todos = await _todoRepository.GetTodosAsync();
            return todos.Select(TodoMapper.ToDto).ToList();
        }

        public async Task<TodoDto?> UpdateTodoAsync(int id, TodoRequest todoRequest)
        {
            var todoEntity = TodoMapper.ToEntity(todoRequest);
            var updatedTodo = await _todoRepository.UpdateTodoAsync(id, todoEntity);

            return updatedTodo == null ? null : TodoMapper.ToDto(updatedTodo);
        }
    }
}
