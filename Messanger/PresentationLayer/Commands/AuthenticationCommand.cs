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
    class AuthenticationCommand : GenericCommand<string>
    {
        private readonly Session _session;
        private readonly Actions _actions;

        public AuthenticationCommand(Session session, Actions actions)
        {
            _session = session;
            _actions = actions;
        }

        public override async Task ExecuteAsync(string action)
        {
            switch (action.Trim().ToLower())
            {
                case ConsolePages.LoginPage:
                    await OpenLoginPage();
                    break;
                case ConsolePages.RegisterPage:
                    await OpenRegisterPage();
                    break;
                case ConsolePages.LogoutPage:
                    OpenLogoutPage();
                    break;
            }
        }

        private async Task OpenLoginPage()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine().Trim();

            Console.Write("Password: ");
            string password = Console.ReadLine().Trim();

            string result = await _actions.ActionLogin(_session, username, password);
            Console.WriteLine($"\n{result}\n");

            if (result.Equals(Status.SuccessfullyLoggedIn))
            {
                string pageContent = string.Concat(
                        "\nGo to:\n\n",
                        "main\n",
                        "logout\n",
                        "exit\n"
                    );

                Console.WriteLine(pageContent);
            }
            else
            {
                Console.WriteLine($"User {username} does not exist or the password is wrong, try again");
            }

        }

        private async Task OpenRegisterPage()
        {
            string pageContent;
            Console.Write("Username: ");
            string username = Console.ReadLine().Trim();

            Console.Write("Email: ");
            string email = Console.ReadLine().Trim();

            Console.Write("Password (any letters, 8-24 length, symbols(!#$%&): ");
            string password = Console.ReadLine().Trim();

            Console.Write("Confirm password: ");
            string confirmPassword = Console.ReadLine().Trim();

            string result = await _actions.ActionRegisterUser(_session, username, password, confirmPassword, email);
            Console.WriteLine($"\n{result}\n");

            if (_session.IsUserLoggedIn)
            {
                pageContent = string.Concat(
                        "Go to:\n\n",
                        "main\n",
                        "logout\n",
                        "exit\n"
                        );
            }
            else
            {
                pageContent = string.Concat(
                        "Go to:\n\n",
                        "login\n",
                        "register\n",
                        "exit\n"
                    );
            }

            Console.WriteLine(pageContent);
        }

        private void OpenLogoutPage()
        {
            string result = _actions.ActionLogout(_session);
            Console.WriteLine($"\n{result}\n");

            string pageContent = string.Concat(
                "\nGo to:\n\n",
                "login\n",
                "register\n",
                "exit\n"
            );

            Console.WriteLine(pageContent);
        }
    }
}
