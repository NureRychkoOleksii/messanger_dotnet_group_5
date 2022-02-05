using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Abstractions.Interfaces;
using Core;
using Core.Models;
using DAL.Abstractions.Interfaces;
using DAL.Services;

namespace BLL.Services
{
    public class RoomUsersService : IRoomUsersService
    {
        private readonly IGenericRepository<RoomUsers> _repository;
        private readonly IUserService _userService;
        private readonly IRoomService _roomService;
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public RoomUsersService(IGenericRepository<RoomUsers> repository, IUserService userService, IRoomService roomService)
        {
            _repository = repository;
            _userService = userService;
            _roomService = roomService;
        }

        public async void CreateRoomUsers(RoomUsers roomUsers)
        {
            _unitOfWork.CreateTransaction();
            
            try
            {
                await _unitOfWork.RoomUsersRepository.Insert(roomUsers);

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
        
        public async void DeleteRoomUsers(RoomUsers roomUsers)
        {
            _unitOfWork.CreateTransaction();
            
            try
            {
                _unitOfWork.RoomUsersRepository.Delete(roomUsers);

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
        
        public async void UpdateRoomUsers(RoomUsers roomUsers)
        {
            _unitOfWork.CreateTransaction();
            
            try
            {
                _unitOfWork.RoomUsersRepository.Update(roomUsers);

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
        
        public async Task<IEnumerable<RoomUsers>> GetRoomUsers()
        {
            IEnumerable<RoomUsers> rooms = null;
            
            _unitOfWork.CreateTransaction();
            
            try
            {
                rooms = await _unitOfWork.RoomUsersRepository.Get();

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
        
        public async Task<IEnumerable<RoomUsers>> GetRoomUsersOfUser(User user)
        {
            IEnumerable<RoomUsers> rooms = null;
            _unitOfWork.CreateTransaction();
            
            try
            {
                var roomsAsync = await _unitOfWork.RoomUsersRepository.Get();

                rooms = roomsAsync.Where(x => x.UserId == user.Id);

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

        public async Task<Role> GetUserRole(User user, Room room)
        {
            IEnumerable<RoomUsers> rooms = null;
            
            try
            {
                var roomsAsync = await _unitOfWork.RoomUsersRepository.Get();

                rooms = roomsAsync.Where(x => x.UserId == user.Id && x.RoomId == room.Id);

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

            int roleId = rooms.FirstOrDefault().RoomId;
            Role role = room.Roles[roleId];
            return role;
            
        }
        
        // public async Task<Role> GetUserRole(User user, Room room)
        // {
        //     IEnumerable<RoomUsers> rooms = null;
        //     
        //     try
        //     {
        //         var roomsAsync = await _unitOfWork.RoomUsersRepository.Get();
        //
        //         rooms = roomsAsync.Where(x => x.UserId == user.Id && x.RoomId == room.Id);
        //
        //         await _unitOfWork.SaveAsync();
        //         
        //         _unitOfWork.Commit();
        //         
        //     }
        //     catch (Exception e)
        //     {
        //         _unitOfWork.RollBack();
        //     }
        //     finally
        //     {
        //         _unitOfWork.Dispose();
        //     }
        //
        //     roleId = rooms.FirstOrDefault().RoomId;
        //     Role role = room.Roles[roleId];
        //     return role;
        // }

        // public async Task<Role> GetUserRole(User user, Room room)
        // {
        //     var roomUserAsync = await _repository
        //         .GetAllAsync(typeof(RoomUsers));
        //     var roomUser = roomUserAsync.Where(roomUser => roomUser.UserId == user.Id && roomUser.RoomId == room.Id)
        //         .FirstOrDefault();
        //     
        //     var userRole = room.Roles
        //         .Where(role => role.Key == roomUser.UserRole)
        //         .FirstOrDefault().Value;
        //
        //     return userRole;
        // }
        
        // public Role GetUserRole(User user, Room room, out int roleId)
        // {
        //     RoomUsers roomUser = _repository
        //         .GetAllAsync(typeof(RoomUsers))
        //         .Result.Where(roomUser => roomUser.UserId == user.Id
        //                                   && roomUser.RoomId == room.Id)
        //         .FirstOrDefault();
        //     
        //     Role userRole = room.Roles
        //         .Where(role => role.Key == roomUser.UserRole)
        //         .FirstOrDefault().Value;
        //
        //     roleId = roomUser.UserRole;
        //     
        //     return userRole;
        // }
        
        
        public async Task<IEnumerable<Room>> GetRoomsOfUser(User user)
        {
            IEnumerable<RoomUsers> roomUsers = null;
            
            try
            {
                var roomsAsync = await _unitOfWork.RoomUsersRepository.Get();

                roomUsers = roomsAsync.Where(x => x.UserId == user.Id);

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

            List<Room> rooms = new List<Room>();
        
            foreach(RoomUsers roomUser in roomUsers)
            {
                var allRooms = await _roomService.GetRooms();
                var room = allRooms.Where(user => user.Id == roomUser.RoomId);
                rooms.Add(room.FirstOrDefault());
            }
            
            
            return rooms;
        }
        
        public async Task<IEnumerable<User>> GetUsersOfRoom(Room room)
        {
            IEnumerable<RoomUsers> roomUsers = null;
            
            try
            {
                var roomsAsync = await _unitOfWork.RoomUsersRepository.Get();

                roomUsers = roomsAsync.Where(x => x.RoomId == room.Id);

                await _unitOfWork.SaveAsync();
                
                _unitOfWork.Commit();
                
            }
            catch (Exception e)
            {
                try
                {
                    _unitOfWork.RollBack();
                }
                catch (Exception e1)
                {
                    
                }
            }

            List<User> users = new List<User>();
        
            foreach (RoomUsers roomUser in roomUsers)
            {
                var userAsync = await _userService.GetUser(user => user.Id == roomUser.Id);
                users.Add(userAsync.FirstOrDefault());
            }
        
            return users;
        }
    }
}