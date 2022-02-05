using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
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
        private readonly Actions _actions;

        public ConsoleInterface(Session session, IUserService userService, IRoomService roomService,
            IRoomUsersService roomUsersService, IEmailService emailService, 
            IUsersInvitationService usersInvitationService, IChatService chatService, Actions actions)
        {
            _session = session;
            _userService = userService;
            _roomService = roomService;
            _roomUsersService = roomUsersService;
            _emailService = emailService;
            _usersInvitationService = usersInvitationService;
            _chatService = chatService;
            _actions = actions;
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
                // case "create role":
                //     OpenCreateRolePage();
                //     break;
                // case "delete role":
                //     OpenDeleteRolePage();
                //     break;
                // case "view roles":
                //     OpenViewRolesPage();
                //     break;
                case "invite user":
                    OpenInvitationPage();
                    break;
                case "my invitations":
                    OpenMyInvitationsPage();
                    break;
                case "update room name":
                    //OpenUpdateRoomNamePage();
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
                    break;
                default:
                    Console.WriteLine($"No such page {action}");
                    break;
            }
        }

        private async void OpenMyInvitationsPage()
        {
            string result;
            Tuple<string, UsersInvitation> checkInvitations = await _actions.ActionCheckInvitations(_session);
            Console.WriteLine($"\n{checkInvitations.Item1}\n");

            if (checkInvitations.Item2 != null)
            {
                var answer = Console.ReadLine().Trim().ToLower();
                result = await _actions.ActionAcceptInvitation(_session, answer, checkInvitations.Item2.RoomId,
                    checkInvitations.Item2.UserId);
                Console.WriteLine($"\n{result}\n");
            }
            //_usersInvitationService.RemoveUser(isUserInvited.Id,_session.CurrentRoom.Id);

        }
        
        private async void OpenInvitationPage()
        {

            Console.WriteLine("Enter a nickname of user or 'exit'");

            string user = Console.ReadLine().Trim();

            string result = await _actions.ActionInviteUser(_session, user);
            Console.WriteLine($"\n{result}\n");
            
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

        private async void OpenLoginPage()
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

        private async void OpenRegisterPage()
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

        private async void OpenCreateRoomPage()
        {

            // create room
            Console.Write("Enter room name: ");
            string roomName = Console.ReadLine().Trim();

            // var condition = await _roomService.RoomExists(roomName);

            string result = await _actions.ActionCreateRoom(_session, roomName);
            
            Console.WriteLine(result);

            Thread.Sleep(1000);

            OpenMainPage();
        }

        private async void OpenViewUserRooms()
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

            string result = await _actions.ActionViewRooms(_session);
            Console.WriteLine($"\n{result}\n");

            Console.WriteLine(pageContent);
        }

        private async void OpenEnterRoomPage()
        {
            string pageContent;
            Console.Write("Enter the name of the room: ");
            string roomName = Console.ReadLine().Trim();
          
            // update room name
            // delete room
            // remove users
            // leave room
            string result = await _actions.ActionEnterRoom(_session, roomName);
            Console.WriteLine($"\n{result}\n");
            
            if (result == Status.SuccessfullyEnteredRoom)
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
            else if (result == Status.NoRoomsFound || result == Status.NoSuchRoomFound)
            {
                // Console.WriteLine($"No room {roomName} in your list of rooms");

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
            else //if (result == Status.UserNotLoggedIn)
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

        public void OpenExitRoomPage()
        {

            string pageContent = string.Concat(
                $"\nHello, {_session.CurrentUser.Nickname}!\n",
                "\nGo to:\n\n",
                "enter room\n",
                "view rooms\n",
                "create room\n",
                "logout\n",
                "exit\n"
            );

            string result = _actions.ActionExitRoom(_session);
            Console.WriteLine($"\n{result}\n");

            Console.WriteLine(pageContent);
        }


        // public async void OpenCreateRolePage()
        // {
        //     if (!_session.IsUserLoggedIn)
        //     {
        //         Console.WriteLine("\nError: not logged in\n\n");
        //         return;
        //     }
        //
        //     Role userRole = await _roomUsersService.GetUserRole(_session.CurrentUser, _session.CurrentRoom);
        //     string pageContent = string.Concat(
        //         "\nGo to:\n\n",
        //         "view users\n",
        //         "exit room\n",
        //         "create role\n",
        //         "delete tole\n",
        //         "view roles\n",
        //         "create chat\n",
        //         "view chats\n",
        //         "enter chat\n"
        //     );
        //     
        //     if (_session.CurrentRoom.Roles[roleId].Permissions["Manage roles"])
        //     {
        //         Console.Write("Enter the name of the new role: ");
        //         string newRoleName = Console.ReadLine().Trim();
        //         
        //         while (String.IsNullOrEmpty(newRoleName))
        //         {
        //             Console.WriteLine("Role name can not be empty.");
        //             Console.Write("Enter the name of the new role: ");
        //             newRoleName = Console.ReadLine().Trim();
        //         }
        //         
        //         bool hasCreatedRole = _roomService.CreateRole(newRoleName, _session.CurrentRoom);
        //
        //         if (hasCreatedRole)
        //         {
        //             Console.WriteLine($"New role {newRoleName} was successfully created!\n");
        //         }
        //         else
        //         {
        //             Console.WriteLine($"Role {newRoleName} already exists.");
        //         }
        //     }
        //     else
        //     {
        //         Console.WriteLine("You don't have permissions to create roles.");
        //     }
        //     
        //     Console.WriteLine(pageContent);
        // }

        // public void OpenDeleteRolePage()
        // {
        //     if (!_session.IsUserLoggedIn)
        //     {
        //         Console.WriteLine("\nError: not logged in\n\n");
        //         return;
        //     }
        //
        //     Role userRole = _roomUsersService.GetUserRole(_session.CurrentUser, _session.CurrentRoom, out int roleId);
        //     string pageContent = string.Concat(
        //         "\nGo to:\n\n",
        //         "view users\n",
        //         "exit room\n",
        //         "create role\n",
        //         "delete tole\n",
        //         "view roles\n",
        //         "create chat\n",
        //         "view chats\n",
        //         "enter chat\n"
        //         );
        //
        //     if (_session.CurrentRoom.Roles[roleId].Permissions["Manage roles"])
        //     {
        //         Console.Write("Enter the name of the role to delete: ");
        //         string roleToDelete = Console.ReadLine().Trim();
        //         bool hasDeletedRole = _roomService.DeleteRole(roleToDelete, _session.CurrentRoom);
        //
        //         if (hasDeletedRole)
        //         {
        //             Console.WriteLine($"Role {roleToDelete} was successfully deleted!");
        //         }
        //         else
        //         {
        //             Console.WriteLine($"No role {roleToDelete} exists.");
        //         }
        //     }
        //     else
        //     {
        //         Console.WriteLine("You don't have permissions to delete roles.");
        //     }
        //     
        //     Console.WriteLine(pageContent);
        // }

        // public void OpenViewRolesPage()
        // {
        //     if (!_session.IsUserLoggedIn)
        //     {
        //         Console.WriteLine("\nError: not logged in\n\n");
        //         return;
        //     }
        //
        //     IEnumerable<Role> roles = _roomService.GetAllRoles(_session.CurrentRoom);
        //     string pageContent = string.Concat(
        //         "\nGo to:\n\n",
        //         "view users\n",
        //         "exit room\n",
        //         "create role\n",
        //         "delete tole\n",
        //         "view roles\n",
        //         "create chat\n",
        //         "view chats\n",
        //         "enter chat\n"
        //     );
        //     
        //     Console.WriteLine();
        //     foreach (Role role in roles)
        //     {
        //         Console.WriteLine(role.RoleName);
        //     }
        //     
        //     Console.WriteLine(pageContent);
        // }
        //
        private async void OpenViewUsersPage()
        {
            string pageContent = string.Empty;
        
            string result = await _actions.ActionViewUsers(_session);
            Console.WriteLine($"\n{result}\n");
            
            if (result != Status.NotInTheRoom && result != Status.UserNotLoggedIn)
            {
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
            else
            {
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
        
            Console.WriteLine(pageContent);
        }

        
        // private async void OpenUpdateRoomNamePage()
        // {
        //     if (!_session.IsUserLoggedIn)
        //     {
        //         Console.WriteLine("\nError: not logged in\n\n");
        //         return;
        //     }
        //
        //     if (_session.CurrentRoom != null)
        //     {
        //         var userRole = await _roomUsersService.GetUserRole(_session.CurrentUser, _session.CurrentRoom);
        //         if (userRole.RoleName == "Admin")
        //         {
        //             Console.Write("Enter new room name: ");
        //             string name = Console.ReadLine().Trim();
        //
        //             while (await _roomService.RoomExists(name))
        //             {
        //                 Console.WriteLine($"Room {name} already exists");
        //                 Console.Write("Enter new room name: ");
        //                 name = Console.ReadLine().Trim();
        //             }
        //
        //             Room room = _session.CurrentRoom;
        //             room.RoomName = name;
        //             _roomService.UpdateRoom(room);
        //             _session.ExitRoom();
        //         }
        //         else
        //         {
        //             Console.WriteLine("\nError: permission denied");
        //         }
        //     }
        //     else
        //     {
        //         Console.WriteLine("\nError: enter a room first");
        //     }
        // }

        public async void OpenViewChatsPage()
        {
            string pageContent;
            string result = await _actions.ActionViewChats(_session);
            Console.WriteLine($"\n{result}\n");
            
            if (result != Status.NotInTheRoom && result != Status.UserNotLoggedIn)
            {
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
            else if (result == Status.NotInTheRoom)
            {
                pageContent = string.Concat(
                    $"\nHello, {_session.CurrentUser.Nickname}!\n",
                    "\nGo to:\n\n",
                    "enter room\n",
                    "view rooms\n",
                    "my invitations\n",
                    "create room\n",
                    "logout\n",
                    "exit\n"
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

        public async void OpenCreateChatPage()
        {
            string pageContent;
            Console.Write("Enter the name of the new chat: ");
            string newChatName = Console.ReadLine().Trim();
                
            Console.Write("Is the chat private? (yes or anything else): ");
            string isPrivate = Console.ReadLine().Trim().ToLower();

            string result = await _actions.ActionCreateChat(_session, newChatName, isPrivate, _session.CurrentRoom.Id);
            Console.WriteLine($"\n{result}\n");
            
            if (result != Status.NotInTheRoom && result != Status.UserNotLoggedIn) 
            {
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
            else if (result == Status.NotInTheRoom)
            {
                pageContent = string.Concat(
                    $"\nHello, {_session.CurrentUser.Nickname}!\n",
                    "\nGo to:\n\n",
                    "enter room\n",
                    "view rooms\n",
                    "my invitations\n",
                    "create room\n",
                    "logout\n",
                    "exit\n"
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

        public async void OpenEnterChatPage()
        {
            string pageContent;
            
            Console.Write("Enter the name of the chat to enter: ");
            string chatName = Console.ReadLine().Trim();

            string result = await _actions.ActionEnterChat(_session, chatName);
            Console.WriteLine($"\n{result}\n");
            
            if (result != Status.NotInTheRoom && result != Status.UserNotLoggedIn) 
            {
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
            else if (result == Status.NotInTheRoom)
            {
                pageContent = string.Concat(
                    $"\nHello, {_session.CurrentUser.Nickname}!\n",
                    "\nGo to:\n\n",
                    "enter room\n",
                    "view rooms\n",
                    "my invitations\n",
                    "create room\n",
                    "logout\n",
                    "exit\n"
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
            
            // if (_session.CurrentRoom != null)
            // {
            //
            //     bool hasEnteredChat = _session.EnterChat(_chatService.GetChat(chatName, _session.CurrentRoom));
            //
            //     if (hasEnteredChat)
            //     {
            //         Console.WriteLine($"Welcome to the chat {chatName}!");
            //         pageContent = string.Concat(
            //             "\nGo to:\n\n",
            //             "exit chat\n"
            //         );
            //     }
            //     else
            //     {
            //         Console.WriteLine($"No chat {chatName} exists in the current room.");
            //
            //         pageContent = string.Concat(
            //             "\nGo to:\n\n",
            //             "view users\n",
            //             "exit room\n",
            //             "create role\n",
            //             "delete tole\n",
            //             "view roles\n",
            //             "view chats\n",
            //             "create chat\n",
            //             "enter chat\n"
            //         );
            //     }
            // }
            // else
            // {
            //     Console.WriteLine("You are not in the room right now.");
            //     pageContent = string.Concat(
            //         $"\nHello, {_session.CurrentUser.Nickname}!\n",
            //         "\nGo to:\n\n",
            //         "view users\n",
            //         "exit room\n",
            //         "create role\n",
            //         "delete tole\n",
            //         "view roles\n",
            //         "create chat\n",
            //         "view chats\n",
            //         "enter chat\n"
            //     );
            // }
        }


        public async void OpenExitChatPage()
        {
            string pageContent;
            string result = _actions.ActionExitChat(_session);
            Console.WriteLine($"\n{result}\n");
            
            if (result != Status.UserNotLoggedIn)
            {
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
    }
}