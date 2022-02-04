using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core;

namespace BLL.Abstractions.Interfaces
{
    public interface IUserService
    {
        void CreateUser(User user);
        void DeleteUser(User user);
        void UpdateUser(User user);

        public Task<IEnumerable<User>> GetUsers();
        // User GetUser(string username);
        // User GetUser(int id);
        public Task<bool> UserExists(Expression<Func<User, bool>> predicate);
        public Task<IEnumerable<User>> GetUser(Expression<Func<User, bool>> predicate);
    }
}