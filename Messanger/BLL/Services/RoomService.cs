using System.Collections.Generic;
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
    }
}