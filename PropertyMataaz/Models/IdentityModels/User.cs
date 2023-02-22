using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Utilities.Constants;

namespace PropertyMataaz.Models
{
    public class User : IdentityUser<int>
    {
        [Required]
        [MaxLength(ModelConstants.MAX_LENGTH_60)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(ModelConstants.MAX_LENGTH_60)]
        public  string LastName { get; set; }
        [Required]
        [MaxLength(ModelConstants.MAX_LENGTH_60)]
        public string Password { get; set; }
        public string Token { get; set; }
        public string PhoneNumber1 { get; set; }
        public string CompanyName {get;set;}
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public IEnumerable<Code> Codes { get; set; }
        public IEnumerable<Report> Reports { get; set; }
        public IEnumerable<Application> Applications { get; set; }
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nationality { get; set; } 
        public string MaritalStatus { get; set; }
        public string Employer  {get;set;}
        public string Occupation { get; set; }
        public string WorkAddress {get;set;}
        public string AnnualIncome { get; set; }
        public int? ProfilePictureId { get; set; }
        public Media ProfilePicture { get; set; }
        public int? PassportPhotographId { get; set; }
        public Media PassportPhotograph { get; set; }
        public int? WorkIdId { get; set; }
        public Media WorkId { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }
        public bool IsAdmin { get; set; }
        public IEnumerable<Property> Properties { get; set; }
        public IEnumerable<UserEnquiry> UserEnquiries { get; set; }
        public IEnumerable<Inspections> Inspections { get; set; }
        public IEnumerable<Cleaning> Cleanings { get; set; }
        public IEnumerable<RentRelief> RentReliefs { get; set; }
        public IEnumerable<Tenancy> Tenancies { get; set; }
        public IEnumerable<Complaints> Complaints { get; set; }
        public IEnumerable<Tenancy> MyTenancies { get; set; }
        public string Bank {get;set;}
        public string AccountNumber { get; set; }

    }
}
