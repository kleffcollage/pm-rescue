using System.Collections.Generic;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Repositories.Interfaces
{
    public interface ITenancyRepository
    {
         Tenancy CreateAndReturn(Tenancy newTenancy);
         IEnumerable<Tenancy> ListTenancy();
         Tenancy Update(Tenancy existingTenancy);
    }
}