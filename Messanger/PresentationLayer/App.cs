using BLL.Abstractions.Interfaces;
using Core;

namespace Messanger
{
    public class App
    {
        private readonly IUserService _userService;

        public App(IUserService userService)
        {
            _userService = userService;
        }

        public void StartApp()
        {
            var x = new User() {Nickname = "Moonler", Password = "1234", Id = 1};
            _userService.CreateUser(x);
        }
    }
}