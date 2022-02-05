using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Core.Models;

namespace BLL.Abstractions.Interfaces
{
    public interface IRoomUsersService
    {
        void CreateRoomUsers(RoomUsers roomUsers);
        void DeleteRoomUsers(RoomUsers roomUsers);
        void UpdateRoomUsers(RoomUsers roomUsers);
        public Task<IEnumerable<RoomUsers>> GetRoomUsers();
        // public Task<Role> GetUserRole(User user, Room room);
        // public Role GetUserRole(User user, Room room, out int roleId);
        public Task<IEnumerable<Room>> GetRoomsOfUser(User user);
        public Task<IEnumerable<User>> GetUsersOfRoom(Room room);
    }
}