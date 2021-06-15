using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zal.Models
{
    /// <summary>
    /// Interface for registering UserRepository as a service for use in dependency injection.
    /// </summary>
    public interface IUserRepository
    {
        Task CreateUser(string userName, string password, string roleType);
        Task<ApplicationUser> GetUser(string id);
        Task<List<ApplicationUser>> GetAllUsers();
        Task DeleteUser(string id);
        Task<bool> CheckAdmins();
    }
}
