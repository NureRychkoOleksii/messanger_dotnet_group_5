using BLL.Abstractions.Interfaces;
using Core;
using System.Threading.Tasks;

namespace Messanger
{
    public class App
    {
        private readonly IUserService _userService;
        private readonly ConsoleInterface _consoleInterface;

        public App(IUserService userService, ConsoleInterface consoleInterface)
        {
            _userService = userService;
            _consoleInterface = consoleInterface;
        }

        public async Task StartApp()
        {
            await _consoleInterface.Start();
        }
    }
}