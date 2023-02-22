using System;

namespace PropertyMataaz.Models.AppModels
{
    public class CleaningQuote : BaseModel
    {
        public double Quote { get; set; }
        public DateTime ProposedDate { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public int CleaningId {get;set;}
        public Cleaning Cleaning { get; set; }
    }
}