using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Helper;

namespace api.Dtos.Todo
{
    public class TodoDto
    {
        public string? Title { get; set; }
        public Status Status { get; set; } = Status.ToDo;
        public string? Description { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}