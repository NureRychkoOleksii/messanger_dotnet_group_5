using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using BLL.Abstractions.Interfaces;
using Core;
using Core.Models;

namespace Messanger
{
    public class ConsoleInterface
    {
        private readonly Session _session;
        private readonly IUserService _userService;
        private readonly IRoomService _roomService;
        private readonly IRoomUsersService _roomUsersService;

        public ConsoleInterface(Session session, IUserService userService, IRoomService roomService, IRoomUsersService roomUsersService)
        {
            _session = session;
            _userService = userService;
            _roomService = roomService;
            _roomUsersService = roomUsersService;
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

        private void ResolveAction(string action)
        {
            switch (action.Trim().ToLower())
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
                case "create room":
                    OpenCreateRoomPage();
                    break;
                case "view rooms":
                    OpenViewUserRooms();
                    break;
                default:
                    Console.WriteLine($"No such page {action}");
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
                    "view rooms\n",
                    "create room\n",
                    "logout\n",
                    "exit\n"
                    );

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

        private void OpenRegisterPage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.Write("Username: ");
                string username = Console.ReadLine();

                while (_userService.UserExists(username))
                {
                    Console.WriteLine($"Name {username} is already taken");
                    Console.Write("Username: ");
                    username = Console.ReadLine();
                }

                //Console.Write("Email: ");
                //string email = Console.ReadLine();

                //// check if email is correct
                //// check if email exists
                ///
                Console.Write("Email: ");
                string email = Console.ReadLine();

                string pageContent;

                Console.Write("Password: ");
                string password = Console.ReadLine();

                // check if password is good enough

                Console.Write("Confirm password: ");
                string confirmPassword = Console.ReadLine();

                while (!password.Equals(confirmPassword))
                {
                    Console.WriteLine("Passwords did not match");
                    Console.Write("Confirm password: ");
                    confirmPassword = Console.ReadLine();
                }

                // hash password

                User user = new User { Nickname = username, Password = password, Email = email};
                _userService.CreateUser(user);

                _session.TryLogin(user.Nickname, user.Password);

                // create register class

                if (_session.IsUserLoggedIn)
                {
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

        private void OpenCreateRoomPage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            // create room
            Console.Write("Enter room name: ");
            string roomName = Console.ReadLine();

            while (_roomService.RoomExists(roomName))
            {
                Console.WriteLine($"Room {roomName} already exists");
                Console.Write("Enter room name: ");
                roomName = Console.ReadLine();
            }

            Room roomToCreate = new Room { RoomName = roomName };
            _roomService.CreateRoom(roomToCreate);

            Room room = _roomService.GetRoom(roomName);

            int userId = _session.CurrentUser.Id;
            int roomId = room.Id;

            RoomUsers roomUser = new RoomUsers { RoomId = roomId, UserId = userId };

            _roomUsersService.CreateRoomUsers(roomUser);

            OpenMainPage();
        }

        private void OpenViewUserRooms()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            var rooms = _roomUsersService.GetRoomsOfUser(_session.CurrentUser);

            Console.WriteLine();

            foreach (Room room in rooms)
            {
                Console.WriteLine(room.RoomName);
            }

            Console.WriteLine();

            Console.Write("Enter room: ");
            string roomName = Console.ReadLine();

            if (rooms.Any(room => room.RoomName == roomName))
            {
                OpenRoom(rooms.Where(room => room.RoomName == roomName).FirstOrDefault());
            }
        }

        private void OpenRoom(Room room)
        {
            Console.WriteLine();

            foreach(User user in _roomUsersService.GetUsersOfRoom(room))
            {
                Console.WriteLine(user.Nickname);
            }

            Console.WriteLine();
        }
    }
}