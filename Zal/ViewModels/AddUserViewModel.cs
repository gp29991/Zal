using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zal.ViewModels
{
    /// <summary>
    /// View model used to validate a user to be added to the database.
    /// Inherits from LoginViewModel, as adding a user utilises the same properties with identical validation.
    /// Additionally adds two new properties for validation of users to be added.
    /// </summary>
    public class AddUserViewModel : LoginViewModel
    {
        [Required(ErrorMessage = "Potwierdzenie hasła jest wymagane")]
        [Display(Name = "Powtórz hasło")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hasła muszą być takie same")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Wybranie roli jest wymagane")]
        [Display(Name = "Rola")]
        public string Role { get; set; }
    }
}
