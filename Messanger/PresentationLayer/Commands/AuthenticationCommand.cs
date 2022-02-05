using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using BLL.Abstractions.Interfaces;
using Core;
using PL.Abstractions.Interfaces;
using Messanger;

namespace PL.Commands
{
    class AuthenticationCommand : GenericCommand<string>
    {
        private readonly Session _session;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public AuthenticationCommand(Session session, IUserService userService, IEmailService emailService)
        {
            _session = session;
            _userService = userService;
            _emailService = emailService;
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
            while (!_session.IsUserLoggedIn)
            {
                Console.Write("Username: ");
                string username = Console.ReadLine().Trim();

                Console.Write("Password: ");
                string password = Console.ReadLine().Trim();

                if (await _session.TryLogin(username, password))
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
        }

        private async Task OpenRegisterPage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.Write("Username: ");
                string username = Console.ReadLine().Trim();

                while (await _userService.UserExists(x => x.Nickname == username))
                {
                    Console.WriteLine($"Name {username} is already taken");
                    Console.Write("Username: ");
                    username = Console.ReadLine().Trim();
                }

                //Console.Write("Email: ");
                //string email = Console.ReadLine();

                //// check if email is correct
                //// check if email exists
                ///
                Console.Write("Email: ");
                string email = Console.ReadLine().Trim();

                string pageContent;

                Console.Write("Password(any letters, 8-24 length, symbols(!#$%&): ");
                string password = Console.ReadLine().Trim();

                // check if password is good enough

                Console.Write("Confirm password: ");
                string confirmPassword = Console.ReadLine().Trim();

                while (!password.Equals(confirmPassword))
                {
                    Console.WriteLine("Passwords did not match");
                    Console.Write("Confirm password: ");
                    confirmPassword = Console.ReadLine().Trim();
                }

                // hash password

                User user = new User { Nickname = username, Password = password, Email = email };
                _userService.CreateUser(user);
                await _session.TryLogin(user.Nickname, user.Password);

                // create register class

                if (_session.IsUserLoggedIn)
                {
                    _emailService.SendingEmailOnRegistration(user);
                    Console.WriteLine("Successfully logged in!\n");
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

        }

        private void OpenLogoutPage()
        {
            _session.TryLogout();

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
