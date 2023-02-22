using System.Collections.Generic;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Services.Interfaces
{
    public interface IReliefService
    {
         StandardResponse<IEnumerable<RentReliefView>> ListMyReliefs();
    }
}