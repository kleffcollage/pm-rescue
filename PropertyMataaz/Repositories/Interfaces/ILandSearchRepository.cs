using System.Collections.Generic;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Repositories.Interfaces
{
    public interface ILandSearchRepository
    {
         LandSearch CreateAndReturn(LandSearch model);
         IEnumerable<LandSearch> List();
    }
}