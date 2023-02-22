using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PropertyMataaz.DataContext;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Repositories.Interfaces;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Repositories
{
    public class PropertyRequestRepository : BaseRepository<PropertyRequest>, IPropertyRequestRepository
    {
        public PropertyRequestRepository(PMContext context) : base(context)
        {
        }

        public PropertyRequest CreateNewRequest(PropertyRequest request)
        {
            try
            {
                var createdRequest = CreateAndReturn(request);
                return createdRequest;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public IEnumerable<PropertyRequest> GetUsersRequests(int userId)
        {
            try
            {
                var requests = _context.PropertyRequests
                .Include(p => p.Matches)
                .Include(p => p.Status)
                .Include(p => p.User)
                .Include(p => p.PropertyType)
                .Where(p => p.UserId == userId);
                var checker = requests.ToList();
                return requests;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public bool RemoveRequestMatch(int id)
        {
            try{
                var thisMatch = _context.PropertyRequestMatches.FirstOrDefault(m => m.Id == id);
                var result =_context.PropertyRequestMatches.Remove(thisMatch);
                _context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }

        public IEnumerable<PropertyRequest> GetRequests()
        {
            try
            {
                var requests = _context.PropertyRequests
                .Include(p => p.Status)
                .Include(p => p.Matches)
                .Include(p => p.User)
                .Include(p => p.PropertyType);

                return requests;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public (bool Succeeded, string ErrorMessage, PropertyRequestMatch Match) AddMatch(PropertyRequestMatch match)
        {
            try
            {
                var result = _context.Add<PropertyRequestMatch>(match);
                _context.SaveChanges();
                return (true, null, match);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return (false, ex.Message, null);
            }
        }
    
        public PropertyRequestMatch UpdateMatch(PropertyRequestMatch match)
        {
            try
            {
                var result = _context.Update<PropertyRequestMatch>(match);
                _context.SaveChanges();
                return match;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }
    
        public PropertyRequestMatch GetMatch(int MatchId)
        {
            try
            {
                var match = _context.PropertyRequestMatches.FirstOrDefault(m => m.Id == MatchId);
                return match;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }
    }
}