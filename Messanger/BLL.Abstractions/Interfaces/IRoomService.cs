using System.Collections.Generic;
using Core;

namespace BLL.Abstractions.Interfaces
{
    public interface IRoomService
    {
        void CreateRoom(Room room);
        void DeleteRoom(Room room);
        void UpdateRoom(Room room);
        IEnumerable<Room> GetRooms();
        Room GetRoom(string name);
        Room GetRoom(int id);
        bool RoomExists(string name);
    }
}