using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using Core.Models;
using PL.Abstractions.Interfaces;
using Messanger;

namespace PL.Commands
{
    class UserInvitationsCommand : GenericCommand<string>
    {
        private readonly Session _session;
        private readonly Actions _actions;

        public UserInvitationsCommand(Session session, Actions actions)
        {
            _session = session;
            _actions = actions;
        }

        public override async Task ExecuteAsync(string action)
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

        private async Task OpenInvitationPage()
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
    }
}
