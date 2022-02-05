using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        
        public async void CreateRoom(Room room)
        {
            await Task.Run(() => _repository.CreateObjectAsync(room));
        }

        public async void DeleteRoom(Room room)
        {
            await _repository.DeleteObjectAsync(room);
        }

        public async void UpdateRoom(Room room)
        {
            await _repository.UpdateObjectAsync(room);
        }

        public async Task<IEnumerable<Room>> GetRooms()
        {
            var rooms = await _repository.GetAllAsync(typeof(Room));

            return rooms;
        }

        public async Task<Room> GetRoom(Func<Room, bool> func)
        {
            var rooms = await _repository
                .GetAllAsync(typeof(Room));
                
            return  rooms.Where(func).FirstOrDefault();
        }

        // public async Task<Room> GetRoom(string roomName)
        // {
        //     var rooms = await _repository.GetAllAsync(typeof(Room));
        //
        //     return rooms.Where(room => room.RoomName == roomName).FirstOrDefault();
        // }

        // public Room GetRoom(int id)
        // {
        //     return _repository
        //         .GetAllAsync(typeof(Room))
        //         .Result.Where(room => room.Id == id)
        //         .FirstOrDefault();
        // }
        //
        public async Task<bool> RoomExists(string name)
        {
            var rooms = await _repository
                .GetAllAsync(typeof(Room));
                
            return rooms.Where(room => room.RoomName == name).FirstOrDefault() != null;
        }

        // public bool CreateRole(string roleName, Room room)
        // {
        //     Role newRole = new Role() {RoleName = roleName};
        //
        //     IList<string> roleNames = new List<string>();
        //
        //     foreach (Role role in room.Roles.Values)
        //     {
        //         roleNames.Add(role.RoleName);
        //     }
        //     
        //     if (!roleNames.Contains(roleName))
        //     {
        //         int roleId = room.Roles.Keys.Last();
        //         room.Roles.Add(++roleId, newRole);
        //         this.UpdateRoom(room);
        //         return true;
        //     }
        //     else
        //     {
        //         return false;
        //     }
        // }
        //
        // public bool DeleteRole(string roleName, Room room)
        // {
        //     IList<string> roleNames = new List<string>();
        //
        //     foreach (Role role in room.Roles.Values)
        //     {
        //         roleNames.Add(role.RoleName);
        //     }
        //     
        //     if (roleNames.Contains(roleName) && roleName != "User" 
        //         && roleName != "Admin")
        //     {
        //         int roleId = room.Roles.Keys.Where(key => room.Roles[key].RoleName == roleName).FirstOrDefault();
        //         room.Roles.Remove(roleId);
        //         this.UpdateRoom(room);
        //         return true;
        //     }
        //     else
        //     {
        //         return false;
        //     }
        // }
        //
        // public IEnumerable<Role> GetAllRoles(Room room)
        // {
        //     IList<Role> roles = new List<Role>();
        //     foreach(Role role in room.Roles.Values)
        //     {
        //         roles.Add(role);
        //     }
        //
        //     return roles;
        // }
        
    }
}