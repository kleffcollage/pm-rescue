using System;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Models.ViewModels
{
    public class ReportView
    {
        public int Id { get; set; }
        public Property Property { get; set; }
        public User User { get; set; }
        public string Description { get; set; }
        public string UsersName { get; set; }
        public DateTime DateCreated { get; set; }
        public string Email { get; set; }
    }
}