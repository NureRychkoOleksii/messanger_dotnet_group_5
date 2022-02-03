using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace BLL.Abstractions.Interfaces
{
    public interface IUsersInvitationService
    {
        void AddUser(int userId,int roomId);
        void RemoveUser(int userId, int roomId);

        Task<IEnumerable<UsersInvitation>> GetUsers();
    }
}