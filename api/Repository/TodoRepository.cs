using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Helper;
using api.Models;
using api.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class TodoRepository : ITodoRepository
    {
        private readonly ApplicationDBContext _context;

        public TodoRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Todo> AddTodoAsync(Todo todo)
        {
            var result = await _context.Todos.AddAsync(todo);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Todo> DeleteTodoAsync(int id)
        {
            var todo = await _context.Todos
                                      .FirstOrDefaultAsync(t => t.Id == id)
                                      ?? throw new Exception("Todo not found");
            todo.IsDeleted = true;
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task<Todo?> GetTodoAsync(int id)
        {
            return await _context.Todos
                                 .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Todo>> GetTodosAsync()
        {
            return await _context.Todos.ToListAsync();
        }

        public async Task<Todo?> UpdateTodoAsync(int id, Todo todo)
        {
            var existingTodo = await _context.Todos
                                              .FirstOrDefaultAsync(t => t.Id == id)
                                              ?? throw new Exception("Todo not found");

            existingTodo.Title = todo.Title ?? existingTodo.Title;
            existingTodo.Description = todo.Description ?? existingTodo.Description;
            if (todo.Status != default(Status))
            {
                existingTodo.Status = todo.Status;
            }
            existingTodo.UserId = todo.UserId ?? existingTodo.UserId;

            await _context.SaveChangesAsync();
            return existingTodo;
        }
    }
}