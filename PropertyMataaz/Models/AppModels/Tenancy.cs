using System;

namespace PropertyMataaz.Models.AppModels
{
    public class Tenancy : BaseModel
    {
        public int TenantId { get; set; }
        public User Tenant { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; }
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }
        public bool Renewable { get; set; }
        public DateTime RentDueDate { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public bool Agreed { get; set; }
    }
}