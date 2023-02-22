using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PropertyMataaz.DataContext;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Repositories.Interfaces;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Repositories
{
    public class RequestRepository : BaseRepository<Request>, IRequestRepository
    {
        public RequestRepository(PMContext context) : base(context)
        {
        }

        public IEnumerable<Request> ListPendingRequests()
        {
            try
            {
                var PendingRequests = ListRequests().Where(r => r.Status.Id == (int)Statuses.PENDING);
                return PendingRequests;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public IEnumerable<Request> ListOnGoingRequests()
        {
            try
            {
                var OngoingRequests = ListRequests().Where(r => r.Status.Id == (int)Statuses.ONGOING);

                return OngoingRequests;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public IEnumerable<Request> ListRequests()
        {
            try
            {
                var Requests = _context.Requests
                                .Include(r => r.Property)
                                .Include(r => r.Status);

                return Requests;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public IEnumerable<Request> ListResolvedRequests()
        {
            try
            {
                var ResolvedRequests = ListRequests().Where(r => r.Status.Id == (int)Statuses.RESOLVED);
                return ResolvedRequests;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public Request New(Request request)
        {
            try
            {
                return CreateAndReturn(request);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }
    }
}