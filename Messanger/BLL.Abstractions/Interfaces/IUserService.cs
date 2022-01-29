using System.Collections.Generic;
using Core;

namespace BLL.Abstractions.Interfaces
{
    public interface IUserService
    {
        void CreateUser(User user);
        void DeleteUser(User user);
        void UpdateUser(User user);
        IEnumerable<User> GetUsers();
        User GetUser(string username);
        User GetUser(int id);
        bool UserExists(string username);
    }
}