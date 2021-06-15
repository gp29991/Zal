using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Zal.Controllers;

namespace Zal.Models
{
    /// <summary>
    /// Repository providing some CRUD functionality (sans update) for the "AspNetUsers" table in the "users" database.
    /// Also provides the method for checking the number of users assigned to the "Administrator" role in the system and creating a default administrator if none exist, used by the Login action of the Home controller.
    /// It uses Identity's UserManager rather than UserContext, so it does not follow the typical repository pattern.
    /// Implements IUserRepository for the purposes of dependency injection.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task CreateUser(string userName, string password, string roleType)
        {
            ApplicationUser user = new ApplicationUser();
            user.UserName = userName;
            user.Role = roleType;
            await _userManager.CreateAsync(user, password);
            await _userManager.AddToRoleAsync(user, roleType);
            await _userManager.AddClaimAsync(user, new Claim("Role", roleType));
        }

        public async Task<ApplicationUser> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;
        }

        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return users;
        }

        public async Task DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }

        public async Task<bool> CheckAdmins()
        {
            var admins = await _userManager.GetUsersInRoleAsync(RoleTypes.Administrator);
            if (admins.Count() == 0)
            {
                await CreateUser("admin", "admin", RoleTypes.Administrator);
                return true;
            }
            return false;
        }
    }
}
