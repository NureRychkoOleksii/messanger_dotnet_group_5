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

        public bool CreateRole(string roleName, Room room)
        {
            Role newRole = new Role() {RoleName = roleName};

            IList<string> roleNames = new List<string>();

            foreach (Role role in room.Roles.Values)
            {
                roleNames.Add(role.RoleName);
            }
            
            if (!roleNames.Contains(roleName))
            {
                int roleId = room.Roles.Keys.Last();
                room.Roles.Add(++roleId, newRole);
                this.UpdateRoom(room);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteRole(string roleName, Room room)
        {
            IList<string> roleNames = new List<string>();

            foreach (Role role in room.Roles.Values)
            {
                roleNames.Add(role.RoleName);
            }
            
            if (roleNames.Contains(roleName) && roleName != "User" 
                && roleName != "Admin")
            {
                int roleId = room.Roles.Keys.Where(key => room.Roles[key].RoleName == roleName).FirstOrDefault();
                room.Roles.Remove(roleId);
                this.UpdateRoom(room);
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<Role> GetAllRoles(Room room)
        {
            IList<Role> roles = new List<Role>();
            foreach(Role role in room.Roles.Values)
            {
                roles.Add(role);
            }

            return roles;
        }
        
    }
}