using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<RoomUsers>> GetRoomUsers()
        {
            var rooms = await _repository.GetAllAsync(typeof(RoomUsers));

            return rooms;
        }

        public IEnumerable<RoomUsers> GetRoomUsersOfUser(User user)
        {
            return _repository
                .GetAllAsync(typeof(RoomUsers))
                .Result.Where(roomUser => roomUser.UserId == user.Id);
        }

        // public string GetUserRole(User user, Room room)
        // {
        //     IList<RoomUsers> roomUser = _repository
        //         .GetAllAsync(typeof(RoomUsers))
        //         .Result.Where(roomUser => roomUser.UserId == user.Id
        //                                   && roomUser.RoomId == room.Id).ToList();
        //     int roleId = roomUser[0].RoomId;
        //     Role role = room.Roles[roleId];
        //     return role.RoleName;
        //
        // }
        //
        // public string GetUserRole(User user, Room room, out int roleId)
        // {
        //     IList<RoomUsers> roomUsers = _repository
        //         .GetAllAsync(typeof(RoomUsers))
        //         .Result.Where(roomUser => roomUser.UserId == user.Id
        //                                   && roomUser.RoomId == room.Id).ToList();
        //     roleId = roomUsers[0].RoomId;
        //     Role role = room.Roles[roleId];
        //     return role.RoleName;
        // }

        public async Task<Role> GetUserRole(User user, Room room)
        {
            var roomUserAsync = await _repository
                .GetAllAsync(typeof(RoomUsers));
            var roomUser = roomUserAsync.Where(roomUser => roomUser.UserId == user.Id && roomUser.RoomId == room.Id)
                .FirstOrDefault();
            
            var userRole = room.Roles
                .Where(role => role.Key == roomUser.UserRole)
                .FirstOrDefault().Value;

            return userRole;
        }
        
        public Role GetUserRole(User user, Room room, out int roleId)
        {
            RoomUsers roomUser = _repository
                .GetAllAsync(typeof(RoomUsers))
                .Result.Where(roomUser => roomUser.UserId == user.Id
                                          && roomUser.RoomId == room.Id)
                .FirstOrDefault();
            
            Role userRole = room.Roles
                .Where(role => role.Key == roomUser.UserRole)
                .FirstOrDefault().Value;

            roleId = roomUser.UserRole;
            
            return userRole;
        }
        

        
        public async Task<IEnumerable<Room>> GetRoomsOfUser(User user)
        {
            var roomUsers = _repository
                .GetAllAsync(typeof(RoomUsers))
                .Result.Where(roomUser => roomUser.UserId == user.Id).ToList();

            List<Room> rooms = new List<Room>();

            foreach(RoomUsers roomUser in roomUsers)
            {
                var room = await _roomService.GetRoom(user => user.Id == roomUser.Id);
                rooms.Add(room);
            }
            
            
            return rooms;
        }

        public async Task<IEnumerable<User>> GetUsersOfRoom(Room room)
        {
            var roomUsers = _repository
                .GetAllAsync(typeof(RoomUsers))
                .Result.Where(roomUser => roomUser.RoomId == room.Id).ToList();

            List<User> users = new List<User>();

            foreach (RoomUsers roomUser in roomUsers)
            {
                var userAsync = await _userService.GetUser(user => user.Id == roomUser.Id);
                users.Add(userAsync);
            }

            return users;
        }
    }
}