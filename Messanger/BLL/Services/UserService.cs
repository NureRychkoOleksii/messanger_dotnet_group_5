using System.Collections;
using System.Collections.Generic;
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
            _repository.CreateObjectAsync(user);
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
    }
}