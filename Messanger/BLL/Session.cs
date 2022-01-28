using System.Linq;
using Core;
using Core.Models;
using BLL.Abstractions.Interfaces;

namespace BLL
{
    public class Session
    {
        private readonly IUserService _userService;

        private bool _isUserLoggedIn = false;
        private User _currentUser;

        public Session(IUserService userService)
        {
            _userService = userService;
        }

        public bool IsUserLoggedIn => _isUserLoggedIn;
        public User CurrentUser => _currentUser;

        public bool TryLogin(string username, string password)
        {
            var users = _userService.GetUsers();
            var user = users.Where(x => x.Nickname == username).FirstOrDefault();

            if (user != null && user.Password == password)
            {
                _isUserLoggedIn = true;
                _currentUser = user;
                return true;
            }

            return false;
        }

        public void TryLogout()
        {
            if (_isUserLoggedIn && _currentUser != null)
            {
                _isUserLoggedIn = false;
                _currentUser = null;
            }
        }
    }
}