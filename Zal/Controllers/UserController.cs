using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zal.Models;
using Zal.ViewModels;

namespace Zal.Controllers
{
    /// <summary>
    /// Controller for dealing with users (viewing, adding, deleting). Accessible only to users assigned to the "Administrator" role.
    /// </summary>
    [Authorize(Roles = RoleTypes.Administrator)]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Method for viewing all users in the database of application users. It gets the list of users from the "users" database, sorts it and displays the appropriate view.
        /// </summary>
        /// <param name="sortBy">Property by which users are sorted.</param>
        /// <param name="sortType">Direction of sorting. "desc" means descending, any other value ("asc" is used as default) means ascending.</param>
        /// <returns>The ViewUsers view with the sorted user list as its model.</returns>
        public async Task<IActionResult> ViewUsers(string sortBy = "UserName", string sortType = "asc")
        {
            ViewBag.title = "Widok Użytkowników";
            ViewMethodViewModel<ApplicationUser> model = new ViewMethodViewModel<ApplicationUser>("UserName", "Role");
            if (sortType != "desc")
            {
                model.SortTypeForColumns[sortBy] = "desc";
            }
            model.Properties = await _userRepository.GetAllUsers();
            switch (sortBy)
            {
                case "Role":
                    model.Properties.Sort((u1, u2) => u1.Role.CompareTo(u2.Role));
                    break;
                default:
                    model.Properties.Sort((u1, u2) => u1.UserName.CompareTo(u2.UserName));
                    break;
            }
            if (sortType == "desc")
            {
                model.Properties.Reverse();
            }
            return View(model);
        }

        /// <summary>
        /// Get method to display the view for adding new users.
        /// </summary>
        /// <returns>The AddUser view.</returns>
        [HttpGet]
        public IActionResult AddUser()
        {
            ViewBag.title = "Dodawanie użytkownika";
            return View();
        }

        /// <summary>
        /// Post method for actually adding new users to the database.
        /// </summary>
        /// <param name="model">Validation model for adding new users.</param>
        /// <returns>Either the AddUser view to display validation problems, or a redirect to the ViewUsers action if the user was successfully added to the database.</returns>
        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserViewModel model)
        {
            ViewBag.title = "Dodawanie użytkownika";
            if (!ModelState.IsValid)
            {
                return View();
            }
            var users = await _userRepository.GetAllUsers();
            if (users.Any(user => user.UserName == model.Username))
            {
                ModelState.AddModelError("UserAlreadyExists", "Użytkownik o podanej nazwie już istnieje");
                return View();
            }
            await _userRepository.CreateUser(model.Username, model.Password, model.Role);
            return RedirectToAction("ViewUsers");
        }

        /// <summary>
        /// Post method for deleting a user from the database. If the user being deleted is currently logged-in, that user will be automatically logged out.
        /// </summary>
        /// <param name="id">The ID of the user to be deleted.</param>
        /// <returns>Either a redirect to the Logout action in the Home controller if the user was logged out, or a redirect to the ViewUsers view if not.</returns>
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            bool deletingLoggedUser = false;
            var user = await _userRepository.GetUser(id);
            if (user.UserName == User.Identity.Name)
            {
                deletingLoggedUser = true;
            }
            await _userRepository.DeleteUser(id);
            if (deletingLoggedUser)
            {
                return RedirectToAction("Logout", "Home");
            }
            return RedirectToAction("ViewUsers");
        }
    }
}
