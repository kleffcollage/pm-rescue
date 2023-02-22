using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyMataaz.Models.ViewModels
{
    public class UserView
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string  CompanyName {get;set;}
        public string Token { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumber1  { get; set; }
        public IEnumerable<PropertyView> Properties { get; set; }
        public MediaView PassportPhotograph { get; set; }
        public MediaView WorkId { get; set; }
        public string AnnualIncome { get; set; }
        public string MaritalStatus { get; set; }
        public string Occupation { get; set; }
        public string Nationality { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string ProfilePicture { get; set; }
        public string Bank {get;set;}
        public string AccountNumber { get; set; }

    }
}
