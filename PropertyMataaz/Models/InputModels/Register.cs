using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyMataaz.Models.InputModels
{
    public class Register
    {
        [Required(ErrorMessage = "An Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }
        public string Password { get; set; }

        [Required(ErrorMessage = "You can not register without a firstname")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "You can not register without a Lastname")]
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumber1 {get;set;}
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public string MaritalStatus { get; set; }
        public string Employer { get; set; }
        public string Occupation { get; set; }
        public string  CompanyName {get;set;}
        public string WorkAddress { get; set; }
        public string AnnualIncome { get; set; }
        public MediaModel PassportPhotograph { get; set; }
        public MediaModel WorkId { get; set;}

    }
}
