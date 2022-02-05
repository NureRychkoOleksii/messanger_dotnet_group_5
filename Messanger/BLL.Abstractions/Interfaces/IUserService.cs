using System;
using System.Collections.Generic;
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
        public Task<bool> UserExists(Func<User, bool> func);
        public Task<User> GetUser(Func<User, bool> func);

        public Task<string> CheckRegisterData(string nickname, string password, string confirmPassword,
            string email);
    }
}