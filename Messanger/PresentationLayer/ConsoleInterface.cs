using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
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
        private readonly IEmailService _emailService;
        private readonly IUsersInvitationService _usersInvitationService;
        private readonly IChatService _chatService;

        public ConsoleInterface(Session session, IUserService userService, IRoomService roomService,
            IRoomUsersService roomUsersService, IEmailService emailService, 
            IUsersInvitationService usersInvitationService, IChatService chatService)
        {
            _session = session;
            _userService = userService;
            _roomService = roomService;
            _roomUsersService = roomUsersService;
            _emailService = emailService;
            _usersInvitationService = usersInvitationService;
            _chatService = chatService;
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
                case "view users":
                    OpenViewUsersPage();
                    break;
                case "enter room":
                    OpenEnterRoomPage();
                    break;
                case "exit room":
                    OpenExitRoomPage();
                    break;
                case "create role":
                    OpenCreateRolePage();
                    break;
                case "delete role":
                    OpenDeleteRolePage();
                    break;
                case "view roles":
                    OpenViewRolesPage();
                    break;
                case "invite user":
                    OpenInvitationPage();
                    break;
                case "my invitations":
                    OpenMyInvitationsPage();
                    break;
                case "update room name":
                    OpenUpdateRoomNamePage();
                    break;
                case "view chats":
                    OpenViewChatsPage();
                    break;
                case "enter chat":
                    OpenEnterChatPage();
                    break;
                case "exit chat":
                    OpenExitChatPage();
                    break;
                case "create chat":
                    OpenCreateChatPage();
                    break;;
                    
                default:
                    Console.WriteLine($"No such page {action}");
                    break;
            }
        }

        private void OpenMyInvitationsPage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }
            
            var usersInvitations = _usersInvitationService.GetUsers().ToList();

            var isUserInvited = usersInvitations.Where(user => _session.CurrentUser.Id == user.UserId).FirstOrDefault();

            var roomName = _roomService.GetRooms().ToList().Where(room => room.Id == isUserInvited.RoomId).FirstOrDefault().RoomName;

            if (isUserInvited != null)
            {
                Console.WriteLine($"You are invited in room {roomName}, do you want to accept this invitation?");
                var answer = Console.ReadLine().ToLower();
                if (answer.Equals("yes"))
                {
                    var roomUsers = new RoomUsers()
                        {RoomId = isUserInvited.RoomId, UserId = isUserInvited.UserId, UserRole = 1};
                    _roomUsersService.CreateRoomUsers(roomUsers);
                   // _usersInvitationService.RemoveUser(isUserInvited.Id,_session.CurrentRoom.Id);
                }
                else
                {
                    Console.WriteLine(":(");
                    //_usersInvitationService.RemoveUser(isUserInvited.Id,_session.CurrentRoom.Id);
                }
            }
            else
            {
                Console.WriteLine("You have zero invitations");
            }
        }
        
        private void OpenInvitationPage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            Console.WriteLine("Enter a nickname of user or 'exit'");

            string user = Console.ReadLine();

            User userToInvite = null;
            userToInvite = _userService.GetUser(user);

            Thread.Sleep(1000);
            // var userToInvite = _userService.GetUser(user);

            if (userToInvite != null)
            {
                _usersInvitationService.AddUser(userToInvite.Id, _session.CurrentRoom.Id);
                _emailService.SendingEmailOnInviting(userToInvite,_session.CurrentRoom.RoomName);
            }
            else
            {
                Console.WriteLine("This user is not existing");
            }
            // string pageContent = String.Empty;
            //
            // if (_session.IsUserLoggedIn)
            // {
            //     pageContent = string.Concat(
            //         "\nGo to:\n\n",
            //         "logout\n"
            //     );
            // }
            // else
            // {
            //     pageContent = string.Concat(
            //         "\nWrite a nickname of user:\n\n",
            //         "invite user\n",
            //         "exit\n"
            //     );
            // }
            //
            // Console.WriteLine(pageContent);
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

        private void OpenLoginPage()
        {
            while (!_session.IsUserLoggedIn)
            {
                Console.Write("Username: ");
                string username = Console.ReadLine().Trim();

                Console.Write("Password: ");
                string password = Console.ReadLine().Trim();

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
                string username = Console.ReadLine().Trim();

                while (_userService.UserExists(username))
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

                User user = new User {Nickname = username, Password = password, Email = email};
                _userService.CreateUser(user);
                _session.TryLogin(user.Nickname, user.Password);

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

        private void OpenCreateRoomPage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            // create room
            Console.Write("Enter room name: ");
            string roomName = Console.ReadLine().Trim();

            while (_roomService.RoomExists(roomName))
            {
                Console.WriteLine($"Room {roomName} already exists");
                Console.Write("Enter room name: ");
                roomName = Console.ReadLine().Trim();
            }

            while (String.IsNullOrEmpty(roomName))
            {
                Console.WriteLine("Room name can not be empty.");
                Console.Write("Enter room name: ");
                roomName = Console.ReadLine().Trim();
            }
            
            Room roomToCreate = new Room {RoomName = roomName};
            _roomService.CreateRoom(roomToCreate);
            
            Console.WriteLine($"Room {roomName} was successfully created!");
            
            Thread.Sleep(1000);

            //Room rooms = _roomService.GetRooms().ToList().LastOrDefault();


            Room room = null;

            while (room == null)
            {
                room = _roomService.GetRoom(roomName);
            }

            
            int userId = _session.CurrentUser.Id;
            int roomId = room.Id;

            RoomUsers roomUser = new RoomUsers {RoomId = roomId, UserId = userId, UserRole = 0};

            _roomUsersService.CreateRoomUsers(roomUser);

            OpenMainPage();
        }

        private void OpenViewUserRooms()
        {
            string pageContent = String.Concat($"\nHello, {_session.CurrentUser.Nickname}!\n",
            "\nGo to:\n\n",
            "enter room\n",
            "view rooms\n",
            "create room\n",
            "invite user\n",
            "logout\n",
            "exit\n"
            );
            
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            var roomUsers = _roomUsersService.GetRoomsOfUser(_session.CurrentUser);
            Thread.Sleep(500);

            Console.WriteLine();
            if (roomUsers.Count() > 0)
            {
                foreach (Room room in roomUsers)
                {
                    Console.WriteLine(room.RoomName);
                }
            }
            else
            {
                Console.WriteLine("You have no rooms.");
            }

            Console.WriteLine(pageContent);
        }

        private void OpenEnterRoomPage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            string pageContent;
            Console.Write("Enter the name of the room: ");
            string roomName = Console.ReadLine().Trim();

            bool hasEnteted = _session.EnterRoom(_roomService.GetRoom(roomName));

            // update room name
            // delete room
            // remove users
            // leave room

            if (hasEnteted)
            {
                Console.WriteLine($"Welcome to the room {_session.CurrentRoom.RoomName}!");
                pageContent = string.Concat(
                    $"\nHello, {_session.CurrentUser.Nickname}!\n",
                    "\nGo to:\n\n",
                    "view users\n",
                    "exit room\n",
                    "create role\n",
                    "delete tole\n",
                    "view roles\n",
                    "invite user\n",
                    "create chat\n",
                    "view chats\n",
                    "update room name\n",
                    "enter chat\n"
                );
            }
            else
            {
                Console.WriteLine($"No room {roomName} in your list of rooms");

                pageContent = string.Concat(
                    $"\nHello, {_session.CurrentUser.Nickname}!\n",
                    "\nGo to:\n\n",
                    "enter room\n",
                    "view rooms\n",
                    "create room\n",
                    "invite user\n",
                    "logout\n",
                    "exit\n"
                );
            }

            Console.WriteLine(pageContent);
        }

        public void OpenExitRoomPage()
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
                "create room\n",
                "logout\n",
                "exit\n"
            );
            bool hasExited = _session.ExitRoom();

            if (hasExited)
            {
                Console.WriteLine("Successfully exited.");
            }
            else
            {
                Console.WriteLine("You are not in the room right now.");
            }

            Console.WriteLine(pageContent);
        }

        public void OpenCreateRolePage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            Role userRole = _roomUsersService.GetUserRole(_session.CurrentUser, _session.CurrentRoom, out int roleId);
            string pageContent = string.Concat(
                "\nGo to:\n\n",
                "view users\n",
                "exit room\n",
                "create role\n",
                "delete tole\n",
                "view roles\n",
                "create chat\n",
                "view chats\n",
                "enter chat\n"
            );
            
            if (_session.CurrentRoom.Roles[roleId].Permissions["Manage roles"])
            {
                Console.Write("Enter the name of the new role: ");
                string newRoleName = Console.ReadLine().Trim();
                
                while (String.IsNullOrEmpty(newRoleName))
                {
                    Console.WriteLine("Role name can not be empty.");
                    Console.Write("Enter the name of the new role: ");
                    newRoleName = Console.ReadLine().Trim();
                }
                
                bool hasCreatedRole = _roomService.CreateRole(newRoleName, _session.CurrentRoom);

                if (hasCreatedRole)
                {
                    Console.WriteLine($"New role {newRoleName} was successfully created!\n");
                }
                else
                {
                    Console.WriteLine($"Role {newRoleName} already exists.");
                }
            }
            else
            {
                Console.WriteLine("You don't have permissions to create roles.");
            }
            
            Console.WriteLine(pageContent);
        }

        public void OpenDeleteRolePage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            Role userRole = _roomUsersService.GetUserRole(_session.CurrentUser, _session.CurrentRoom, out int roleId);
            string pageContent = string.Concat(
                "\nGo to:\n\n",
                "view users\n",
                "exit room\n",
                "create role\n",
                "delete tole\n",
                "view roles\n",
                "create chat\n",
                "view chats\n",
                "enter chat\n"
                );

            if (_session.CurrentRoom.Roles[roleId].Permissions["Manage roles"])
            {
                Console.Write("Enter the name of the role to delete: ");
                string roleToDelete = Console.ReadLine().Trim();
                bool hasDeletedRole = _roomService.DeleteRole(roleToDelete, _session.CurrentRoom);

                if (hasDeletedRole)
                {
                    Console.WriteLine($"Role {roleToDelete} was successfully deleted!");
                }
                else
                {
                    Console.WriteLine($"No role {roleToDelete} exists.");
                }
            }
            else
            {
                Console.WriteLine("You don't have permissions to delete roles.");
            }
            
            Console.WriteLine(pageContent);
        }

        public void OpenViewRolesPage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            IEnumerable<Role> roles = _roomService.GetAllRoles(_session.CurrentRoom);
            string pageContent = string.Concat(
                "\nGo to:\n\n",
                "view users\n",
                "exit room\n",
                "create role\n",
                "delete tole\n",
                "view roles\n",
                "create chat\n",
                "view chats\n",
                "enter chat\n"
            );
            
            Console.WriteLine();
            foreach (Role role in roles)
            {
                Console.WriteLine(role.RoleName);
            }
            
            Console.WriteLine(pageContent);
        }

        private void OpenViewUsersPage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            string pageContent = string.Empty;

            if (_session.CurrentRoom != null)
            {
                var users = _roomUsersService.GetUsersOfRoom(_session.CurrentRoom);

                Console.WriteLine();
                foreach(User user in users)
                {
                    Console.WriteLine(user.Nickname);
                }
                Console.WriteLine();

                pageContent = string.Concat(
                    "\nGo to:\n\n",
                    "view users\n",
                    "exit room\n",
                    "create role\n",
                    "delete tole\n",
                    "view roles\n",
                    "create chat\n",
                    "view chats\n",
                    "enter chat\n"
                );
            }
            else
            {
                Console.WriteLine("\nError: enter a room first");

                pageContent = string.Concat(
                    $"\nHello, {_session.CurrentUser.Nickname}!\n",
                    "\nGo to:\n\n",
                    "enter room\n",
                    "view rooms\n",
                    "create room\n",
                    "logout\n",
                    "exit\n"
                );
            }

            Console.WriteLine(pageContent);
        }

        private void OpenUpdateRoomNamePage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            if (_session.CurrentRoom != null)
            {
                if (_roomUsersService.GetUserRole(_session.CurrentUser, _session.CurrentRoom).RoleName == "Admin")
                {
                    Console.Write("Enter new room name: ");
                    string name = Console.ReadLine().Trim();

                    while (_roomService.RoomExists(name))
                    {
                        Console.WriteLine($"Room {name} already exists");
                        Console.Write("Enter new room name: ");
                        name = Console.ReadLine().Trim();
                    }

                    Room room = _session.CurrentRoom;
                    room.RoomName = name;
                    _roomService.UpdateRoom(room);
                    _session.ExitRoom();
                }
                else
                {
                    Console.WriteLine("\nError: permission denied");
                }
            }
            else
            {
                Console.WriteLine("\nError: enter a room first");
            }
        }

        public void OpenViewChatsPage()
        {
            string pageContent = string.Concat(
                "\nGo to:\n\n",
                "view users\n",
                "exit room\n",
                "create role\n",
                "delete tole\n",
                "view roles\n",
                "view chats\n",
                "create chat\n",
                "enter chat\n"
            );
            
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            if (_session.CurrentRoom != null)
            {
                var chats = _chatService.GetChats(_session.CurrentRoom);

                if (chats.Count() > 0)
                {
                    Console.WriteLine();

                    foreach (Chat chat in chats)
                    {
                        Console.WriteLine(chat.Name);
                    }

                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("\nYou have no chats.");
                }
            }
            else
            {
                Console.WriteLine("You are not in the room right now.");
            }
            
            Console.WriteLine(pageContent);
        }

        public void OpenCreateChatPage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }
            
            if (_session.CurrentRoom != null)
            {
                string pageContent = string.Concat(
                    "\nGo to:\n\n",
                    "view users\n",
                    "exit room\n",
                    "create role\n",
                    "delete tole\n",
                    "view roles\n",
                    "view chats\n",
                    "create chat\n",
                    "enter chat\n"
                );
                
                Console.Write("Enter the name of the new chat: ");
                string newChatName = Console.ReadLine().Trim();

                while (String.IsNullOrEmpty(newChatName))
                {
                    Console.WriteLine("Chat name can not be empty.");
                    Console.Write("Enter the name of the new chat: ");
                    newChatName = Console.ReadLine().Trim();
                }
                
                Console.Write("Is the chat private? (yes / no): ");
                string isPrivate = Console.ReadLine().Trim().ToLower();
                
                while (!isPrivate.Equals("yes") && !isPrivate.Equals("no"))
                {
                    Console.WriteLine("Wrong. Type \"yes\" or \"no\" ");
                    isPrivate = Console.ReadLine().Trim();
                }

                bool isChatPrivate = isPrivate.Equals("yes") ? true : false;

                Chat newChat = new Chat() {Name = newChatName, IsPrivate = isChatPrivate, 
                    RoomId = _session.CurrentRoom.Id};

                bool hasChatCreated = _chatService.CreateChat(newChat, _session.CurrentRoom);

                if (hasChatCreated)
                {
                    Console.WriteLine($"Chat {newChatName} successfully created!");
                }
                else
                {
                    Console.WriteLine($"Chat {newChatName} already exists.");
                }

                Console.WriteLine(pageContent);
            }
            else
            {
                Console.WriteLine("You are not in the room right now.");
            }
            
        }

        public void OpenEnterChatPage()
        {
            string pageContent;
            
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            if (_session.CurrentRoom != null)
            {
                Console.Write("Enter the name of the chat to enter: ");
                string chatName = Console.ReadLine().Trim();

                bool hasEnteredChat = _session.EnterChat(_chatService.GetChat(chatName, _session.CurrentRoom));

                if (hasEnteredChat)
                {
                    Console.WriteLine($"Welcome to the chat {chatName}!");
                    pageContent = string.Concat(
                        "\nGo to:\n\n",
                        "exit chat\n"
                    );
                }
                else
                {
                    Console.WriteLine($"No chat {chatName} exists in the current room.");

                    pageContent = string.Concat(
                        "\nGo to:\n\n",
                        "view users\n",
                        "exit room\n",
                        "create role\n",
                        "delete tole\n",
                        "view roles\n",
                        "view chats\n",
                        "create chat\n",
                        "enter chat\n"
                    );
                }
            }
            else
            {
                Console.WriteLine("You are not in the room right now.");
                pageContent = string.Concat(
                    $"\nHello, {_session.CurrentUser.Nickname}!\n",
                    "\nGo to:\n\n",
                    "view users\n",
                    "exit room\n",
                    "create role\n",
                    "delete tole\n",
                    "view roles\n",
                    "create chat\n",
                    "view chats\n",
                    "enter chat\n"
                );
            }
            Console.WriteLine(pageContent);
        }

        public void OpenExitChatPage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            string pageContent = string.Concat(
                "\nGo to:\n\n",
                "view users\n",
                "exit room\n",
                "create role\n",
                "delete tole\n",
                "view roles\n",
                "view chats\n",
                "create chat\n",
                "enter chat\n"
            );

            bool hasExitedChat = _session.ExitChat();

            if (hasExitedChat)
            {
                Console.WriteLine("Successfully exited!");
            }
            else
            {
                Console.WriteLine("You are not in the chat right now.");
            }
            
            Console.WriteLine(pageContent);
        }
    }
}