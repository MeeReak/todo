using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace C_App.Service.Interface
{
    public interface ITokenService
    {
        string CreateToken(IdentityUser user);
    }
}