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
        Role GetUserRole(User user, Room room);
        public Role GetUserRole(User user, Room room, out int roleId);
        IEnumerable<Room> GetRoomsOfUser(User user);
        IEnumerable<User> GetUsersOfRoom(Room room);
    }
}