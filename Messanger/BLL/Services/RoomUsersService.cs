using System.Collections.Generic;
using System.Linq;
using BLL.Abstractions.Interfaces;
using Core;
using Core.Models;
using DAL.Abstractions.Interfaces;

namespace BLL.Services
{
    public class RoomUsersService : IRoomUsersService
    {
        private readonly IRepository<RoomUsers> _repository;
        private readonly IUserService _userService;
        private readonly IRoomService _roomService;

        public RoomUsersService(IRepository<RoomUsers> repository, IUserService userService, IRoomService roomService)
        {
            _repository = repository;
            _userService = userService;
            _roomService = roomService;
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

        public IEnumerable<Room> GetRoomsOfUser(User user)
        {
            var roomUsers = _repository
                .GetAllAsync(typeof(RoomUsers))
                .Result.Where(roomUser => roomUser.UserId == user.Id);

            List<Room> rooms = new List<Room>();

            foreach(RoomUsers roomUser in roomUsers)
            {
                rooms.Add(_roomService.GetRoom(roomUser.RoomId));
            }

            return rooms;
        }

        public IEnumerable<User> GetUsersOfRoom(Room room)
        {
            var roomUsers = _repository
                .GetAllAsync(typeof(RoomUsers))
                .Result.Where(roomUser => roomUser.RoomId == room.Id);

            List<User> users = new List<User>();

            foreach (RoomUsers roomUser in roomUsers)
            {
                users.Add(_userService.GetUser(roomUser.UserId));
            }

            return users;
        }
    }
}