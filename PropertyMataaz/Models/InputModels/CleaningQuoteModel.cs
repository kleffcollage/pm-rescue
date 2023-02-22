using System;

namespace PropertyMataaz.Models.InputModels
{
    public class CleaningQuoteModel
    {
        public double Quote { get; set; }
        public DateTime ProposedDate { get; set; }
        public int CleaningId { get; set; }
    }
}