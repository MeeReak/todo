using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Auth
{
    // DTO Models
    public class Register
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}