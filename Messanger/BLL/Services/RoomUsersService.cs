using System.Collections.Generic;
using BLL.Abstractions.Interfaces;
using Core.Models;
using DAL.Abstractions.Interfaces;

namespace BLL.Services
{
    public class RoomUsersService : IRoomUsersService
    {
        private readonly IRepository<RoomUsers> _repository;

        public RoomUsersService(IRepository<RoomUsers> repository)
        {
            _repository = repository;
        }

        public void CreateRoomUsers(RoomUsers roomUsers)
        {
            _repository.CreateObjectAsync(roomUsers);
        }

        public void DeleteRoomUsers(RoomUsers roomUsers)
        {
            _repository.DeleteObjectAsync(roomUsers);
        }

        public void UpdateRoomUsers(RoomUsers roomUsers)
        {
            _repository.UpdateObjectAsync(roomUsers);
        }

        public IEnumerable<RoomUsers> GetRoomUsers()
        {
            return _repository.GetAllAsync(typeof(RoomUsers)).Result;
        }
    }
}