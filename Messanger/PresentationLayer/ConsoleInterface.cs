using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using BLL.Abstractions.Interfaces;

namespace Messanger
{
    public class ConsoleInterface
    {
        private readonly Session _session;

        public ConsoleInterface(Session session)
        {
            _session = session;
        }

        public void Start()
        {
            string action = "start";
            
            while (!action.Equals("exit"))
            {
                ResolveAction(action);

                Console.Write("Choose one option: ");
                action = Console.ReadLine();
            }
        }

        private void OpenStartPage()
        {
            string pageContent = string.Empty;
            
            if (_session.IsUserLoggedIn)
            {
                pageContent = string.Concat(
                    "Go to:\n\n",
                    "logout\n"
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

        private void OpenLoginPage()
        {
            while (!_session.IsUserLoggedIn)
            {
                Console.Write("Username: ");
                string username = Console.ReadLine();

                Console.Write("Password: ");
                string password = Console.ReadLine();

                if (_session.TryLogin(username, password))
                {
                    string pageContent = string.Concat(
                    "Go to:\n\n",
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

        private void OpenLogoutPage()
        {
            _session.TryLogout();

            string pageContent = string.Concat(
                    "Go to:\n\n",
                    "login\n",
                    "register\n",
                    "exit\n"
                    );

            Console.WriteLine(pageContent);
        }

        private void OpenRegisterPage()
        {
            //if (!_session.IsUserLoggedIn)
            //{
            //    Console.Write("Username: ");
            //    string username = string.Empty;

            //    while (true)
            //    {
            //        username = Console.ReadLine();
            //    }

            //    // check if user exists

            //    Console.Write("Email: ");
            //    string email = Console.ReadLine();

            //    // check if email is correct
            //    // check if email exists

            //    Console.Write("Password: ");
            //    string password = Console.ReadLine();

            //    Console.Write("Confirm password: ");
            //    string confirmPassword = Console.ReadLine();

            //    // check if passwords match
            //    // check if password is good enough

            //    // hash password
            //    // create new user
            //    // log in
            //}
        }

        private void ResolveAction(string action)
        {
            switch (action)
            {
                case "start":
                    OpenStartPage();
                    break;
                case "main":
                    OpenMainPage();
                    break;
                case "login":
                    OpenLoginPage();
                    break;
                case "register":
                    OpenRegisterPage();
                    break;
                case "logout":
                    OpenLogoutPage();
                    break;
                default:
                    Console.WriteLine($"No such page {action}");
                    break;
            }
        }

        private void OpenMainPage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("Error: not logged in");
                return;
            }

            Console.WriteLine("Main");
        }
    }
}
