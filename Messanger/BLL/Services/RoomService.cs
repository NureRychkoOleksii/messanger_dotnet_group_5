using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BLL.Abstractions.Interfaces;
using Core;
using DAL.Abstractions.Interfaces;
using DAL.Services;

namespace BLL.Services
{
    public class RoomService : IRoomService
    {
        private readonly IGenericRepository<Room> _repository;

        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        
        public RoomService(IGenericRepository<Room> repository)
        {
            _repository = repository;
        }
        
        public async void CreateRoom(Room room)
        {

            _unitOfWork.CreateTransaction();
            
            try
            {
                await Task.Run(() => _unitOfWork.RoomRepository.Insert(room));

                await _unitOfWork.SaveAsync();
                
                _unitOfWork.Commit();
                
            }
            catch (Exception e)
            {
                try
                {
                    _unitOfWork.RollBack();
                }
                catch(Exception e1)
                {
                    
                }
            }
        }
        
        public async void DeleteRoom(Room room)
        {
            _unitOfWork.CreateTransaction();
            
            try
            {
                _unitOfWork.RoomRepository.Delete(room);

                await _unitOfWork.SaveAsync();
                
                _unitOfWork.Commit();
                
            }
            catch (Exception e)
            {
                try
                {
                    _unitOfWork.RollBack();
                }
                catch(Exception e1)
                {
                    
                }
            }
        }
        
        public async void UpdateRoom(Room room)
        {
            _unitOfWork.CreateTransaction();
            
            try
            {
                _unitOfWork.RoomRepository.Update(room);

                await _unitOfWork.SaveAsync();
                
                _unitOfWork.Commit();
                
            }
            catch (Exception e)
            {
                try
                {
                    _unitOfWork.RollBack();
                }
                catch(Exception e1)
                {
                    
                }
            }
        }
        
        public async Task<IEnumerable<Room>> GetRooms()
        {
            IEnumerable<Room> rooms = null;
            _unitOfWork.CreateTransaction();
            
            try
            {
                rooms = await _unitOfWork.RoomRepository.Get();

                //await _unitOfWork.SaveAsync();
                
                _unitOfWork.Commit();
                
            }
            catch (Exception e)
            {
                try
                {
                    _unitOfWork.RollBack();
                }
                catch(Exception e1)
                {
                    
                }
            }

            return rooms;
        }
        
        public async Task<IEnumerable<Room>> GetRoom(Expression<Func<Room, bool>> predicate)
        {
            IEnumerable<Room> rooms = null;
            
            _unitOfWork.CreateTransaction();
            
            try
            {
                rooms = await _unitOfWork.RoomRepository.Get(predicate);
                
                await _unitOfWork.SaveAsync();
                
                _unitOfWork.Commit();
                
            }
            catch (Exception e)
            {
                try
                {
                    _unitOfWork.RollBack();
                }
                catch(Exception e1)
                {
                    
                }
            }

            return rooms;
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
            IEnumerable<Room> rooms = null;
            
            _unitOfWork.CreateTransaction();
            
            try
            {
                rooms = await _unitOfWork.RoomRepository.Get(x => x.RoomName == name);

                await _unitOfWork.SaveAsync();
                
                _unitOfWork.Commit();
                
            }
            catch (Exception e)
            {
                try
                {
                    _unitOfWork.RollBack();
                }
                catch(Exception e1)
                {
                    
                }
            }

            return rooms.FirstOrDefault() != null;
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