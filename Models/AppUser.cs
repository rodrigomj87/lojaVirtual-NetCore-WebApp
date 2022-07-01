using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApp.Models
{
    public class AppUser : IdentityUser
    {
        public string Nome { get; set; }
    }
}
