using System.Collections.Generic;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Repositories.Interfaces
{
    public interface IComplaintsRepository
    {
         Complaints CreateAndReturn(Complaints model);
         Complaints Update(Complaints model);
         IEnumerable<Complaints> ListComplaints();
    }
}