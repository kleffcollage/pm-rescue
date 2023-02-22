using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyMataaz.Models.InputModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "An Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A password is required to log in.")]
        public string Password { get; set; }
    }
}
