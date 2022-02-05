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
using PL.Abstractions.Interfaces;
using PL.Commands;

namespace Messanger
{
    public class ConsoleInterface
    {
        private readonly Session _session;
        private readonly Actions _actions;

        private Dictionary<string, GenericCommand<string>> _actionPageMap = new Dictionary<string, GenericCommand<string>>();

        public ConsoleInterface(Session session, Actions actions)
        {
            _session = session;
            _actions = actions;

            InitActionPageMap();
        }

        public async Task Start()
        {
            string action = "start";

            while (!action.Equals("exit"))
            {
                await ResolveActionAsync(action);
                Console.Write("Choose one option: ");
                action = Console.ReadLine();
            }
        }

        private void InitActionPageMap()
        {
            MainPagesCommand mainPagesCommand = new MainPagesCommand(_session);
            AuthenticationCommand authenticationCommand = new AuthenticationCommand(_session, _actions);
            RoomsCommand roomsCommand = new RoomsCommand(_session, _actions);
            //RolesCommand rolesCommand = new RolesCommand(_session, _roomService, _roomUsersService);
            ChatsCommand chatsCommand = new ChatsCommand(_session, _actions);
            UserInvitationsCommand userInvitationsCommand = new UserInvitationsCommand(_session, _actions);

            _actionPageMap.Add(ConsolePages.StartPage, mainPagesCommand);
            _actionPageMap.Add(ConsolePages.MainPage, mainPagesCommand);

            _actionPageMap.Add(ConsolePages.LoginPage, authenticationCommand);
            _actionPageMap.Add(ConsolePages.RegisterPage, authenticationCommand);
            _actionPageMap.Add(ConsolePages.LogoutPage, authenticationCommand);

            _actionPageMap.Add(ConsolePages.CreateRoomPage, roomsCommand);
            _actionPageMap.Add(ConsolePages.ViewRoomsPage, roomsCommand);
            _actionPageMap.Add(ConsolePages.ViewUsersPage, roomsCommand);
            _actionPageMap.Add(ConsolePages.EnterRoomPage, roomsCommand);
            _actionPageMap.Add(ConsolePages.ExitRoomPage, roomsCommand);
            //_actionPageMap.Add(ConsolePages.UpdateRoomName, roomsCommand);

            //_actionPageMap.Add(ConsolePages.CreateRolePage, rolesCommand);
            //_actionPageMap.Add(ConsolePages.ViewRolesPage, rolesCommand);
            //_actionPageMap.Add(ConsolePages.DeleteRolePage, rolesCommand);

            _actionPageMap.Add(ConsolePages.CreateChatPage, chatsCommand);
            _actionPageMap.Add(ConsolePages.ViewChatsPage, chatsCommand);
            _actionPageMap.Add(ConsolePages.EnterChatPage, chatsCommand);
            _actionPageMap.Add(ConsolePages.ExitChatPage, chatsCommand);

            _actionPageMap.Add(ConsolePages.InviteUserPage, userInvitationsCommand);
            _actionPageMap.Add(ConsolePages.MyInvitationsPage, userInvitationsCommand);
        }

        private async Task ResolveActionAsync(string action)
        {
            string trimmedAction = action.Trim().ToLower();

            if (_actionPageMap.ContainsKey(trimmedAction))
            {
                await _actionPageMap[trimmedAction].ExecuteAsync(trimmedAction);
            }
            else
            {
                Console.WriteLine($"\nNo such page {action}\n\n");
            }
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
    }
}