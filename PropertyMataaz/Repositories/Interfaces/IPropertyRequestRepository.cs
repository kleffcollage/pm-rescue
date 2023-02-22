using System.Collections.Generic;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Repositories.Interfaces
{
    public interface IPropertyRequestRepository
    {
         IEnumerable<PropertyRequest> GetUsersRequests(int userId);
         PropertyRequest CreateAndReturn(PropertyRequest request);
         PropertyRequest CreateNewRequest(PropertyRequest request);
         (bool Succeeded, string ErrorMessage, PropertyRequestMatch Match) AddMatch(PropertyRequestMatch match);
         IEnumerable<PropertyRequest> GetRequests();
         bool RemoveRequestMatch(int id);
         PropertyRequestMatch UpdateMatch(PropertyRequestMatch match);
         PropertyRequestMatch GetMatch(int MatchId);
    }
}