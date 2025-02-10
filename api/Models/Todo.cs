using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using api.Helper;

namespace api.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; } = Status.ToDo;
        public string? Description { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int? UserId { get; set; }

    }
}