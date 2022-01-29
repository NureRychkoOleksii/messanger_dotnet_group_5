using System.Collections;
using System.Collections.Generic;
using Core.Models;

namespace BLL.Abstractions.Interfaces
{
    public interface IUsersInvitationService
    {
        void AddUser(int userId,int roomId);
        void RemoveUser(int userId, int roomId);

        IEnumerable<UsersInvitation> GetUsers();
    }
}