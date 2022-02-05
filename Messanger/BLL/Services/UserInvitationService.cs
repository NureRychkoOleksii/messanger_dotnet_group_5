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
    public class UserInvitationService : IUsersInvitationService
    {
        private readonly IGenericRepository<UsersInvitation> _repository;

        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        
        public UserInvitationService(IGenericRepository<UsersInvitation> repository)
        {
            _repository = repository;
        }
        
        public async void AddUser(int userId, int roomId)
        {
            var user = new UsersInvitation() {RoomId = roomId, UserId = userId,};
            
            _unitOfWork.CreateTransaction();
            
            try
            {
                await _unitOfWork.UserInvitationRepository.Insert(user);

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
        
        public async void RemoveUser(int userId, int roomId)
        {
            var user = new UsersInvitation() {UserId = userId, RoomId = roomId};
        
            _unitOfWork.CreateTransaction();
            
            try
            {
                _unitOfWork.UserInvitationRepository.Delete(user);

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
        
        public async Task<IEnumerable<UsersInvitation>> GetUsers()
        {
            IEnumerable<UsersInvitation> users = null;
            
            _unitOfWork.CreateTransaction();
            
            try
            {
                users = await _unitOfWork.UserInvitationRepository.Get();

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

            return users;
        }
    }
}