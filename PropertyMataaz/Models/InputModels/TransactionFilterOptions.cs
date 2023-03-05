using System;

namespace PropertyMataaz.Models.InputModels
{
    public class TransactionFilterOptions
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }
}