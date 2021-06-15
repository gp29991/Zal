using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zal.Controllers
{
    /// <summary>
    /// Controller for user roles. Its only purpose is to ensure that the hardcoded roles from the RoleTypes static class exist in the "users" database.
    /// </summary>
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        /// <summary>
        /// This method ensures that all hard-coded roles are accounted for in the "users" database.
        /// It is called by the Index action of the Home controller when the application website is first accessed.
        /// As the roles are hard-coded, no other role management options (adding/editing/deleting) are provided by the application.
        /// </summary>
        /// <returns>A redirect to the Login action of the Home controller</returns>
        public async Task<IActionResult> CheckRoles()
        {
            foreach (var i in RoleTypes.list)
            {
                if (!await _roleManager.RoleExistsAsync(i))
                {
                    IdentityRole role = new IdentityRole(i);
                    await _roleManager.CreateAsync(role);
                }
            }
            return RedirectToAction("Login", "Home");
        }
    }

    /// <summary>
    /// This static class contains the hard-coded roles available for users of the application.
    /// The list is used by the CheckRoles action in the Roles controller and by the AddUser view to construct a selectable list of user roles.
    /// Individual strings are used by the "Authorize" data annotations.
    /// </summary>
    public static class RoleTypes
    {
        public const string Administrator = "Administrator";
        public const string Manager = "Kierownik";
        public const string User = "Użytkownik";
        public static readonly List<string> list = new List<string>()
        {
            RoleTypes.Administrator,
            RoleTypes.Manager,
            RoleTypes.User,
        };
    }
}
