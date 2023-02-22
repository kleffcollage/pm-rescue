
using System.Collections.Generic;

namespace PropertyMataaz.Models.ViewModels
{
    public class RentReliefView
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public PropertyView Property { get; set; }
        public UserView User { get; set; }
        public IEnumerable<InstallmentView> Installments { get; set; }
        public string Status { get; set; }
        public int Interest { get; set; }
        public double MonthlyInstallment { get; set; }
        public double TotalRepayment { get; set; }
        public double ReliefAmount { get; set; }
    }
}