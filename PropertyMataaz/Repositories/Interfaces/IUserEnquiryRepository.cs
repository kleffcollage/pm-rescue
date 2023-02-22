using System.Collections.Generic;
using System.Linq;
using PropertyMataaz.Models.AppModels;

namespace PropertyMataaz.Repositories.Interfaces
{
    public interface IUserEnquiryRepository
    {
        IEnumerable<UserEnquiry> ListUserActiveEnquiries(int UserId);
        UserEnquiry GetById(int Id);
        IQueryable<UserEnquiry> ListAllActiveEnquiries();
        UserEnquiry Update(UserEnquiry enquiry);
        IQueryable<UserEnquiry> Query();
    }
}