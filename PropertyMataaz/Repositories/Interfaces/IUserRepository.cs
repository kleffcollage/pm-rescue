using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PropertyMataaz.Models;

namespace PropertyMataaz.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<(bool Succeeded, string ErrorMessage, User CreatedUser)> CreateUser(User newUser);

        public Task<(bool Succeeded,string ErrorMessage, User LoggedInUser)> Authenticate(User userToLogin);
        public Task<(bool Succeeded, IQueryable<User> Users)> ListUsers();
        Task<(bool Suceeded, string ErrorMessage)> DeleteUser(User user);
        Task<(bool Succeeded, string ErrorMessage)> AddEnquiry(int PropertyId, int UserId);
        User LoggedInUser();
        int UsersCount();
        int NewUsersCount();
        Task<bool> CancelEnquiry(int PropertyId, int UserId);
    }
}
