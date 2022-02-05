using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using PL.Abstractions.Interfaces;
using Messanger;

namespace PL.Commands
{
    class MainPagesCommand : GenericCommand<string>
    {
        private readonly Session _session;

        public MainPagesCommand(Session session)
        {
            _session = session;
        }

        public async Task ExecuteAsync(string action)
        {
            switch (action.Trim().ToLower())
            {
                case ConsolePages.StartPage:
                    OpenStartPage();
                    break;
                case ConsolePages.MainPage:
                    OpenMainPage();
                    break;
            }
        }

        private void OpenStartPage()
        {
            string pageContent = string.Empty;

            if (_session.IsUserLoggedIn)
            {
                pageContent = string.Concat(
                    "\nGo to:\n\n",
                    "logout\n"
                );
            }
            else
            {
                pageContent = string.Concat(
                    "\nGo to:\n\n",
                    "login\n",
                    "register\n",
                    "exit\n"
                );
            }

            Console.WriteLine(pageContent);
        }

        private void OpenMainPage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            string pageContent = string.Concat(
                $"\nHello, {_session.CurrentUser.Nickname}!\n",
                "\nGo to:\n\n",
                "enter room\n",
                "view rooms\n",
                "my invitations\n",
                "create room\n",
                "logout\n",
                "exit\n"
            );

            Console.WriteLine(pageContent);
        }
    }
}
