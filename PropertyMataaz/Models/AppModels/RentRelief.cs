using System.Collections.Generic;

namespace PropertyMataaz.Models.AppModels
{
    public class RentRelief : BaseModel
    {
        public int PropertyId { get; set; }
        public Property Property { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public IEnumerable<Installment> Installments { get; set; }
    }
}