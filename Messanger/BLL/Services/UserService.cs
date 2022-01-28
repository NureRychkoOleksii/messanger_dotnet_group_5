using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        public void CreateUser(User user)
        {
            if (CheckRegisterData(user.Nickname, user.Password, user.Email))
            {
                _repository.CreateObjectAsync(user);
            }
        }

        public void DeleteUser(User user)
        {
            _repository.DeleteObjectAsync(user);
        }

        public void UpdateUser(User user)
        {
            _repository.UpdateObjectAsync(user);
        }

        public IEnumerable<User> GetUsers()
        {
           return _repository.GetAllAsync(typeof(User)).Result;
        }
        
        public User GetUserByName(string username)
        {
            return _repository
                .GetAllAsync(typeof(User)).
                Result.Where(user => user.Nickname == username).
                FirstOrDefault();
        }

        public bool UserExists(string username)
        {
            return _repository
                .GetAllAsync(typeof(User))
                .Result.Where(user => user.Nickname == username)
                .FirstOrDefault() != null;
        }
        
        public bool CheckRegisterData (string nickname, string password, string email)
        {
            var users = this.GetUsers();
            var userName = users.Where(user => user.Nickname == nickname).FirstOrDefault();
            string checkPassword = new string(@"^[a-zA-Z0-9!#$%&]{8,24}$");
            string checkEmail = new string( @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (userName != null || String.IsNullOrEmpty(nickname))
            {
                Console.WriteLine("The given nickname is not unique.\n");
                return false;
            }
            else if (!Regex.IsMatch(email, checkEmail))
            {
                Console.WriteLine("Invalid email.\n");
                return false;
            }
            else if (!Regex.IsMatch(password, checkPassword))
            {
                Console.WriteLine("Invalid password.\n");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}