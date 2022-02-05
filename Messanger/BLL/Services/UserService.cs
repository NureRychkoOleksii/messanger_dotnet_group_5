using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BLL.Abstractions.Interfaces;
using Core;
using DAL.Abstractions.Interfaces;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repository;

        public UserService(IRepository<User> repository)
        {
            _repository = repository;
        }

        public async void CreateUser(User user)
        {
            await _repository.CreateObjectAsync(user);
        }

        public async void DeleteUser(User user)
        {
            await _repository.DeleteObjectAsync(user);
        }

        public async void UpdateUser(User user)
        {
            await _repository.UpdateObjectAsync(user);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _repository.GetAllAsync(typeof(User));

            return users;
        }

        public async Task<User> GetUser(Func<User,bool> func)
        {
           var users =  await _repository.GetAllAsync(typeof(User));
               
           return users.Where(func).FirstOrDefault();
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
        public async Task<bool> UserExists(Func<User, bool> func)
        {
            var user =  await _repository.GetAllAsync(typeof(User));
            
            return user.Where(func).FirstOrDefault() != null;
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