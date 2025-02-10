using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Todo;
using api.Models;

namespace api.Mappers
{
    public class TodoMapper
    {
        public static Todo ToEntity(TodoRequest request)
        {
            return new Todo
            {
                Title = request.Title,
                Status = request.Status,
                Description = request.Description
            };
        }

        public static TodoDto ToDto(Todo todo)
        {
            return new TodoDto
            {
                Title = todo.Title,
                Status = todo.Status,
                Description = todo.Description,
                IsDeleted = todo.IsDeleted
            };
        }
    }
}