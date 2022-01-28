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
        User GetUserByName(string username);
        bool UserExists(string username);
    }
}