namespace PropertyMataaz.Models.AppModels
{
    public class Status : BaseModel
    {
        public string Name { get; set; }
    }

    enum Statuses
    {
        PENDING = 1,
        APPROVED,
        ONGOING,
        RESOLVED,
        VERIFIED,
        DRAFTED,
        ACTIVE,
        INACTIVE,
        REJECTED,
        SOLD,
        COMPLETED,
        ACCEPTED,
        REVIEWED,
        TERMINATED
    }
}