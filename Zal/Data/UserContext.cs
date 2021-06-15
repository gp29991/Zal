using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zal.Models;

namespace Zal.Data
{
    /// <summary>
    /// Database context for the "users" database
    /// </summary>
    public class UserContext : IdentityDbContext<ApplicationUser>
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }
    }
}
