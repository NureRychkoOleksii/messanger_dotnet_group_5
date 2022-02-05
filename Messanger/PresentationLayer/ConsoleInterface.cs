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
using PL.Abstractions.Interfaces;
using PL.Commands;

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

        private Dictionary<string, GenericCommand<string>> _actionPageMap = new Dictionary<string, GenericCommand<string>>();

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

            InitActionPageMap();
        }

        public async Task Start()
        {
            string action = "start";

            while (!action.Equals("exit"))
            {
                await ResolveAction(action);

                Console.Write("Choose one option: ");
                action = Console.ReadLine();
            }
        }

        private void InitActionPageMap()
        {
            MainPagesCommand mainPagesCommand = new MainPagesCommand(_session);
            AuthenticationCommand authenticationCommand = new AuthenticationCommand(_session, _userService, _emailService);
            RoomsCommand roomsCommand = new RoomsCommand(_session, _roomService, _roomUsersService);
            RolesCommand rolesCommand = new RolesCommand(_session, _roomService, _roomUsersService);
            ChatsCommand chatsCommand = new ChatsCommand(_session, _chatService);
            UserInvitationsCommand userInvitationsCommand = new UserInvitationsCommand(_session, _userService, _roomService,
                _roomUsersService, _usersInvitationService, _emailService);

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
            _actionPageMap.Add(ConsolePages.UpdateRoomName, roomsCommand);

            _actionPageMap.Add(ConsolePages.CreateRolePage, rolesCommand);
            _actionPageMap.Add(ConsolePages.ViewRolesPage, rolesCommand);
            _actionPageMap.Add(ConsolePages.DeleteRolePage, rolesCommand);

            _actionPageMap.Add(ConsolePages.CreateChatPage, chatsCommand);
            _actionPageMap.Add(ConsolePages.ViewChatsPage, chatsCommand);
            _actionPageMap.Add(ConsolePages.EnterRoomPage, chatsCommand);
            _actionPageMap.Add(ConsolePages.ExitChatPage, chatsCommand);

            _actionPageMap.Add(ConsolePages.InviteUserPage, userInvitationsCommand);
            _actionPageMap.Add(ConsolePages.MyInvitationsPage, userInvitationsCommand);
        }

        private async Task ResolveAction(string action)
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

    }
}