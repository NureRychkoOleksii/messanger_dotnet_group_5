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
    }
}