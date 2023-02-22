using System.ComponentModel.DataAnnotations;

namespace PropertyMataaz.Models.InputModels
{
    public class ReportModel
    {
        [Required(ErrorMessage = "A property is required to create a report")]
        public int PropertyId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
    }
}