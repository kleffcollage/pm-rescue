using System.Collections.Generic;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Repositories.Interfaces
{
    public interface IRequestRepository 
    {
        Request New(Request request);
        IEnumerable<Request> ListRequests();
        IEnumerable<Request> ListPendingRequests();
        IEnumerable<Request> ListOnGoingRequests();
        IEnumerable<Request> ListResolvedRequests();
    }
}