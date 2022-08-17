using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core;

namespace BLL.Abstractions.Interfaces
{
    public interface IUserService
    {
        Task CreateUser(User user);
        Task DeleteUser(User user);
        Task UpdateUser(User user);

        public Task<IEnumerable<User>> GetUsers();
        // User GetUser(string username);
        // User GetUser(int id);
        public Task<bool> UserExists(Expression<Func<User, bool>> predicate);
        public Task<IEnumerable<User>> GetUser(Expression<Func<User, bool>> predicate);
        public Task<string> CheckRegisterData(string nickname, string password, string confirmPassword,
            string email);
    }
}