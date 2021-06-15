using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zal.Models
{
    /// <summary>
    /// Model class for the "AspNetUsers" table in the "users" database. Inherits from IdentityUser.
    /// Only needed because ASP.NET core doesn't provide a straightforward way to get a user's role.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }
    }
}
