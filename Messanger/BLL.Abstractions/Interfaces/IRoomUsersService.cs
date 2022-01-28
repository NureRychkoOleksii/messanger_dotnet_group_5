using System.Collections.Generic;
using Core;
using Core.Models;

namespace BLL.Abstractions.Interfaces
{
    public interface IRoomUsersService
    {
        void CreateRoomUsers(RoomUsers roomUsers);
        void DeleteRoomUsers(RoomUsers roomUsers);
        void UpdateRoomUsers(RoomUsers roomUsers);
        IEnumerable<RoomUsers> GetRoomUsers();
    }
}