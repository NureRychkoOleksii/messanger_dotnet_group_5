using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BLL.Abstractions.Interfaces;
using Core;
using DAL.Abstractions.Interfaces;
using DAL.Services;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _genericRepository;

        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public UserService(IGenericRepository<User> repository)
        {
            _genericRepository = repository;
        }

        public async void CreateUser(User user)
        {

            _unitOfWork.CreateTransaction();
            
            try
            {
                await _unitOfWork.UserRepository.Insert(user);

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

        public async void DeleteUser(User user)
        {
            _unitOfWork.CreateTransaction();
            
            try
            {
                _unitOfWork.UserRepository.Delete(user);

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

        public async void UpdateUser(User user)
        {
            _unitOfWork.CreateTransaction();
            
            try
            {
                _unitOfWork.UserRepository.Update(user);

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

        public async Task<IEnumerable<User>> GetUsers()
        {
            IEnumerable<User> users = null;
            
            _unitOfWork.CreateTransaction();
            
            try
            {
                users = await _unitOfWork.UserRepository.Get();

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

            return users;
        }

        public async Task<IEnumerable<User>> GetUser(Expression<Func<User,bool>> predicate)
        {
            IEnumerable<User> user = null;
            
            _unitOfWork.CreateTransaction();
            
            try
            {
                user = await _unitOfWork.UserRepository.Get(predicate);

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

            return user;
        }
        
        // public User GetUser(string username)
        // {
        //     return _repository
        //         .GetAllAsync(typeof(User))
        //         .Result.Where(user => user.Nickname == username)
        //         .FirstOrDefault();
        // }
        //
        // public User GetUser(int id)
        // {
        //     return _repository
        //         .GetAllAsync(typeof(User))
        //         .Result.Where(user => user.Id == id)
        //         .FirstOrDefault();
        // }
        // 
        public async Task<bool> UserExists(Expression<Func<User, bool>> predicate)
        {
            IEnumerable<User> user = null;
            
            _unitOfWork.CreateTransaction();
            
            try
            {
                user = await _unitOfWork.UserRepository.Get(predicate);

               // await _unitOfWork.SaveAsync();
                
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

            return user.FirstOrDefault() != null;
            
        }
        
        public async Task<string> CheckRegisterData (string nickname, string password, string confirmPassword, 
            string email)
        {
            var users = await this.GetUsers();
            var userName = users.Where(user => user.Nickname == nickname).FirstOrDefault();
            string checkPassword = new string(@"^[a-zA-Z0-9!#$%&]{8,24}$");
            string checkEmail = new string( @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (userName != null || String.IsNullOrEmpty(nickname))
            {
                return Status.UserNameNotUnique;
            }
            else if (!Regex.IsMatch(email, checkEmail))
            {
                return Status.InvalidEmail;
            }
            else if (!Regex.IsMatch(password, checkPassword))
            {
                return Status.InvalidPassword;
            }
            else if (!password.Equals(confirmPassword))
            {
                return Status.PasswordsNotMatch;
            }
            else
            {
                return Status.StatusOk;
            }
        }
    }
}