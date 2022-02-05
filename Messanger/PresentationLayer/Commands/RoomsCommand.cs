using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BLL;
using Core;
using PL.Abstractions.Interfaces;
using Messanger;

namespace PL.Commands
{
    class RoomsCommand : GenericCommand<string>
    {
        private readonly Session _session;
        private readonly Actions _actions;

        public RoomsCommand(Session session, Actions actions)
        {
            _session = session;
            _actions = actions;
        }

        public override async Task ExecuteAsync(string action)
        {
            switch (action.Trim().ToLower())
            {
                case ConsolePages.CreateRoomPage:
                    await OpenCreateRoomPage();
                    break;
                case ConsolePages.ViewRoomsPage:
                    await OpenViewUserRooms();
                    break;
                case ConsolePages.EnterRoomPage:
                    await OpenEnterRoomPage();
                    break;
                case ConsolePages.ExitRoomPage:
                    OpenExitRoomPage();
                    break;
                case ConsolePages.ViewUsersPage:
                    await OpenViewUsersPage();
                    break;
                //case ConsolePages.UpdateRoomName:
                //    await OpenUpdateRoomNamePage();
                //    break;
            }
        }

        private async Task OpenCreateRoomPage()
        {

            // create room
            Console.Write("Enter room name: ");
            string roomName = Console.ReadLine().Trim();

            // var condition = await _roomService.RoomExists(roomName);

            string result = await _actions.ActionCreateRoom(_session, roomName);

            Console.WriteLine(result);

            Thread.Sleep(1000);
        }

        private async Task OpenViewUserRooms()
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

        private async Task OpenEnterRoomPage()
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

        private async Task OpenViewUsersPage()
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

        //private async Task OpenUpdateRoomNamePage()
        //{
        //    if (!_session.IsUserLoggedIn)
        //    {
        //        Console.WriteLine("\nError: not logged in\n\n");
        //        return;
        //    }

        //    if (_session.CurrentRoom != null)
        //    {
        //        var userRole = await _roomUsersService.GetUserRole(_session.CurrentUser, _session.CurrentRoom);
        //        if (userRole.RoleName == "Admin")
        //        {
        //            Console.Write("Enter new room name: ");
        //            string name = Console.ReadLine().Trim();

        //            while (await _roomService.RoomExists(name))
        //            {
        //                Console.WriteLine($"Room {name} already exists");
        //                Console.Write("Enter new room name: ");
        //                name = Console.ReadLine().Trim();
        //            }

        //            Room room = _session.CurrentRoom;
        //            room.RoomName = name;
        //            _roomService.UpdateRoom(room);
        //            _session.ExitRoom();
        //        }
        //        else
        //        {
        //            Console.WriteLine("\nError: permission denied");
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("\nError: enter a room first");
        //    }
        //}
    }
}
