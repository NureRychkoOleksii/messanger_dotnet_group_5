using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BLL;
using BLL.Abstractions.Interfaces;
using Core;
using Core.Models;
using PL.Abstractions.Interfaces;
using Messanger;

namespace PL.Commands
{
    class RoomsCommand : GenericCommand<string>
    {
        private readonly Session _session;
        private readonly IRoomService _roomService;
        private readonly IRoomUsersService _roomUsersService;

        public RoomsCommand(Session session, IRoomService roomService, IRoomUsersService roomUsersService)
        {
            _session = session;
            _roomService = roomService;
            _roomUsersService = roomUsersService;
        }

        public async Task ExecuteAsync(string action)
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
                case ConsolePages.UpdateRoomName:
                    await OpenUpdateRoomNamePage();
                    break;
            }
        }

        private async Task OpenCreateRoomPage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            // create room
            Console.Write("Enter room name: ");
            string roomName = Console.ReadLine().Trim();

            var condition = await _roomService.RoomExists(roomName);

            while (condition)
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

            Room roomToCreate = new Room { RoomName = roomName };
            _roomService.CreateRoom(roomToCreate);

            Console.WriteLine($"Room {roomName} was successfully created!");

            Thread.Sleep(1000);

            //Room rooms = _roomService.GetRooms().ToList().LastOrDefault();


            Room room = null;

            while (room == null)
            {
                room = await _roomService.GetRoom(x => x.RoomName == roomName);
            }


            int userId = _session.CurrentUser.Id;
            int roomId = room.Id;

            RoomUsers roomUser = new RoomUsers { RoomId = roomId, UserId = userId, UserRole = 0 };

            _roomUsersService.CreateRoomUsers(roomUser);
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

            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            var roomUsers = await _roomUsersService.GetRoomsOfUser(_session.CurrentUser);

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

        private async Task OpenEnterRoomPage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            string pageContent;
            Console.Write("Enter the name of the room: ");
            string roomName = Console.ReadLine().Trim();

            var room = await _roomService.GetRoom(x => x.RoomName == roomName);

            bool hasEnteted = await _session.EnterRoom(room);

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

        private async Task OpenViewUsersPage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            string pageContent = string.Empty;

            if (_session.CurrentRoom != null)
            {
                var users = await _roomUsersService.GetUsersOfRoom(_session.CurrentRoom);

                Console.WriteLine();
                foreach (User user in users)
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

        private async Task OpenUpdateRoomNamePage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            if (_session.CurrentRoom != null)
            {
                var userRole = await _roomUsersService.GetUserRole(_session.CurrentUser, _session.CurrentRoom);
                if (userRole.RoleName == "Admin")
                {
                    Console.Write("Enter new room name: ");
                    string name = Console.ReadLine().Trim();

                    while (await _roomService.RoomExists(name))
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
    }
}
