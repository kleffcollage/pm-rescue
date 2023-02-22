using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PropertyMataaz.DataContext;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Repositories.Interfaces;

namespace PropertyMataaz.Repositories
{
    public class UserEnquiryRepository : BaseRepository<UserEnquiry>, IUserEnquiryRepository
    {
        public UserEnquiryRepository(PMContext context) : base(context)
        {
        }

        public IEnumerable<UserEnquiry> ListUserActiveEnquiries(int UserId)
        {
            var userEnquiries = _context.UserEnquiries.Where(e => e.UserId == UserId && e.Active)
                                                      .Include(e => e.User)
                                                      .Include(e => e.Property)
                                                      .Include(e => e.Property.MediaFiles)
                                                      .Include(e => e.Property.PropertyType);
            return userEnquiries;
        }

        public IQueryable<UserEnquiry> ListAllActiveEnquiries()
        {
            var enquiries = _context.UserEnquiries.Where(e => e.Active)
                                                  .Include(e => e.User)
                                                  .Include(e => e.Property);
            return enquiries;
        }
    }
}