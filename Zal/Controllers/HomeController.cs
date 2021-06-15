using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Zal.Models;
using Zal.ViewModels;

namespace Zal.Controllers
{
    /// <summary>
    /// The default controller loaded by the application. Deals with initialization tasks as well as with logging the users in and out.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(IUserRepository userRepository, SignInManager<ApplicationUser> signInManager)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
        }

        /// <summary>
        /// The deafult action loaded by the application.
        /// It redirects to the CheckRoles method of the Role controller to ensure that all hard-coded roles are present in the "users" database.
        /// </summary>
        /// <returns>A redirect to the CheckRoles method of the Role controller</returns>
        public IActionResult Index()
        {
            return RedirectToAction("CheckRoles", "Role");
        }

        /// <summary>
        /// Get method for displaying the Login view. It also ensures that there always is at least one user with the "Administrator" role in the system.
        /// </summary>
        /// <returns>The Login view</returns>
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var defaultAdminCreated = await _userRepository.CheckAdmins();
            ViewBag.defaultAdminCreated = defaultAdminCreated;
            ViewBag.title = "Strona Startowa";
            return View();
        }

        /// <summary>
        /// Post method for actually logging the user in.
        /// </summary>
        /// <param name="model">Validation model for user login.</param>
        /// <returns>Either the Login view to display validation problems or a redirect to the ViewEmployees view of the Employee controller if the login was successful.</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("BadUsernameOrPassword", "Nieprawidłowa nazwa użytkownika lub hasło");
                return View();
            }
            return RedirectToAction("ViewEmployees", "Employee");
        }

        /// <summary>
        /// Method for logging the user out.
        /// </summary>
        /// <returns>Redirect to the Login view.</returns>
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        /// <summary>
        /// Method for displaying the Access Denied page if a logged-in user tried to access a page not accessible to his/her role.
        /// </summary>
        /// <returns>The AccessDenied view.</returns>
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
