using System;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Models.ViewModels
{
    public class TenancyView
    {
        public int Id { get; set; }
        public User Tenant { get; set; }
        public User Owner { get; set; }
        public Property Property { get; set; }
        public Transaction Transaction { get; set; }
        public DateTime RentDueDate { get; set; }
        public string Status { get; set; }
        public bool Renewable { get; set; }
        public bool Agreed { get; set; }
    }
}