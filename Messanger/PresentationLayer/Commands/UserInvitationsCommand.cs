using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using BLL.Abstractions.Interfaces;
using Core.Models;
using PL.Abstractions.Interfaces;
using Messanger;
using System.Threading;

namespace PL.Commands
{
    class UserInvitationsCommand : GenericCommand<string>
    {
        private readonly Session _session;
        private readonly IUserService _userService;
        private readonly IRoomService _roomService;
        private readonly IRoomUsersService _roomUsersService;
        private readonly IUsersInvitationService _usersInvitationService;
        private readonly IEmailService _emailService;

        public UserInvitationsCommand(Session session, IUserService userService, IRoomService roomService,
            IRoomUsersService roomUsersService, IUsersInvitationService usersInvitationService, IEmailService emailService)
        {
            _session = session;
            _userService = userService;
            _roomService = roomService;
            _roomUsersService = roomUsersService;
            _usersInvitationService = usersInvitationService;
            _emailService = emailService;
        }

        public async Task ExecuteAsync(string action)
        {
            switch (action.Trim().ToLower())
            {
                case ConsolePages.MyInvitationsPage:
                    await OpenMyInvitationsPage();
                    break;
                case ConsolePages.InviteUserPage:
                    await OpenInvitationPage();
                    break;
            }
        }

        private async Task OpenMyInvitationsPage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            var usersInvitationsAsync = await _usersInvitationService.GetUsers();
            var usersInvitations = usersInvitationsAsync.ToList();

            var isUserInvited = usersInvitations.Where(user => _session.CurrentUser.Id == user.UserId).FirstOrDefault();

            var rooms = await _roomService.GetRooms();
            var roomName = rooms.ToList().Where(room => room.Id == isUserInvited.RoomId).FirstOrDefault().RoomName;

            if (isUserInvited != null)
            {
                Console.WriteLine($"You are invited in room {roomName}, do you want to accept this invitation?");
                var answer = Console.ReadLine().ToLower();
                if (answer.Equals("yes"))
                {
                    var roomUsers = new RoomUsers()
                    { RoomId = isUserInvited.RoomId, UserId = isUserInvited.UserId, UserRole = 1 };
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

        private async Task OpenInvitationPage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            Console.WriteLine("Enter a nickname of user or 'exit'");

            string user = Console.ReadLine();

            var userToInvite = await _userService.GetUser(userUser => userUser.Nickname == user);

            Thread.Sleep(1000);
            // var userToInvite = _userService.GetUser(user);

            if (userToInvite is not null)
            {
                _usersInvitationService.AddUser(userToInvite.Id, _session.CurrentRoom.Id);
                _emailService.SendingEmailOnInviting(userToInvite, _session.CurrentRoom.RoomName);
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
    }
}
