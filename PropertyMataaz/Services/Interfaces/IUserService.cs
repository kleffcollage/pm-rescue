using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyMataaz.Models;
using PropertyMataaz.Models.AppModels;
using PropertyMataaz.Models.InputModels;
using PropertyMataaz.Models.UtilityModels;
using PropertyMataaz.Models.ViewModels;
using PropertyMataaz.Utilities;

namespace PropertyMataaz.Services.Interfaces
{
    public interface IUserService
    {
        public StandardResponse<UserView> CreateUser(Register newUser);

        public StandardResponse<UserView> Authenticate(LoginModel userToLogin);

        public StandardResponse<PagedCollection<UserView>> ListUsers(PagingOptions pagingOptions, string search);

        StandardResponse<UserView> DeleteUser(string email);
        public StandardResponse<UserView> VerifyUser(string token, string email);

        StandardResponse<UserView> InitiatePasswordReset(string email);

        StandardResponse<UserView> GetUser(int userId);

        StandardResponse<UserView> CompletePasswordReset(PasswordReset payload);

        StandardResponse<UserView> ResetPassword(PasswordResetModel payload);
        StandardResponse<UserView> AddEnquiry(int propertyId);
        StandardResponse<UserView> CancelEnquiry(int propertyId);
        StandardResponse<PagedCollection<UserEnquiry>> ListUserEnquiries(PagingOptions pagingOptions);
        StandardResponse<UserEnquiryView> GetEnquiryById(int Id);
        StandardResponse<PagedCollection<UserEnquiryView>> ListAllEnquiries(PagingOptions pagingOptions);
        StandardResponse<UserView> CreateAdminUser(Register newUser);
        public StandardResponse<UserView> AuthenticateAdmin(LoginModel userToLogin);
        StandardResponse<PagedCollection<UserView>> ListAllAAdminUsers(PagingOptions pagingOptions, string search);
        StandardResponse<MetricsView> Metrics();
        StandardResponse<UserView> UpdateUser(UpdateUserModel model);
        StandardResponse<UserView> RequestVerificationMail(string email);
    }
}
