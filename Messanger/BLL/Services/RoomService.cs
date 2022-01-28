using System.Collections.Generic;
using System.Linq;
using BLL.Abstractions.Interfaces;
using Core;
using DAL.Abstractions.Interfaces;

namespace BLL.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRepository<Room> _repository;

        public RoomService(IRepository<Room> repository)
        {
            _repository = repository;
        }
        
        public void CreateRoom(Room room)
        {
            _repository.CreateObjectAsync(room);
        }

        public void DeleteRoom(Room room)
        {
            _repository.DeleteObjectAsync(room);
        }

        public void UpdateRoom(Room room)
        {
            _repository.UpdateObjectAsync(room);
        }

        public IEnumerable<Room> GetRooms()
        {
            return _repository.GetAllAsync(typeof(Room)).Result;
        }

        public Room GetRoom(string name)
        {
            return _repository
                .GetAllAsync(typeof(Room))
                .Result.Where(room => room.RoomName == name)
                .FirstOrDefault();
        }

        public Room GetRoom(int id)
        {
            return _repository
                .GetAllAsync(typeof(Room))
                .Result.Where(room => room.Id == id)
                .FirstOrDefault();
        }

        public bool RoomExists(string name)
        {
            return _repository
                .GetAllAsync(typeof(Room))
                .Result.Where(room => room.RoomName == name)
                .FirstOrDefault() != null;
        }
    }
}