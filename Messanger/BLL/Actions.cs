using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BLL.Abstractions;
using BLL.Abstractions.Interfaces;
using Core;
using Core.Models;

namespace BLL
{
    public class Actions //: IActions
    {
        private readonly Session _session;
        private readonly IUserService _userService;
        private readonly IRoomService _roomService;
        private readonly IRoomUsersService _roomUsersService;
        private readonly IEmailService _emailService;
        private readonly IUsersInvitationService _usersInvitationService;
        private readonly IChatService _chatService;

        public Actions(Session session, IUserService userService, IRoomService roomService,
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

        public async Task<string> ActionRegisterUser(Session session, string userName, string password, 
            string confirmPassword, string email)
        {
            string dataCheck;
            
            if (!session.IsUserLoggedIn)
            {
                dataCheck = await _userService.CheckRegisterData(userName, password, confirmPassword, email);
            }
            else
            {
                return Status.AlreadyLoggedIn;
            }

            if (dataCheck.Equals(Status.StatusOk))
            {
                var newUser = new User() {Email = email, Nickname = userName, Password = password};
                _userService.CreateUser(newUser);
                session.TryLogin(userName, password);
                _emailService.SendingEmailOnRegistration(newUser);
                return Status.SuccessfullyLoggedIn;
            }

            return dataCheck;

        }

        public async Task<string> ActionLogin(Session session, string userName, string password)
        {
            bool hasLoggedIn = await session.TryLogin(userName, password);

            if (hasLoggedIn)
            {
                return Status.SuccessfullyLoggedIn;
            }
            
            return Status.InvalidLoginData;
            
        }

        public string ActionLogout(Session session)
        {
            if (session.IsUserLoggedIn)
            {
                session.TryLogout();
                return Status.SuccessfullyLoggedOut;
            }

            return Status.UserNotLoggedIn;
        }

        public async Task<string> ActionCreateRoom(Session session, string roomName)
        {
            if (!session.IsUserLoggedIn)
            {
                return Status.UserNotLoggedIn;
            }
            
            bool roomExists = await _roomService.RoomExists(roomName);

            if (roomExists)
            {
                return Status.RoomAlreadyExists;
            }

            if (String.IsNullOrEmpty(roomName))
            {
                return Status.RoomNameEmpty;
            }

            await Task.Run(() => _roomService.CreateRoom(new Room() {RoomName = roomName}));
            
            Thread.Sleep(1000);
            
            Room createdRoom = await _roomService.GetRoom(someRoom => someRoom.RoomName == roomName);

            RoomUsers roomUsers = new RoomUsers() {RoomId = createdRoom.Id, UserId = session.CurrentUser.Id};
            
            _roomUsersService.CreateRoomUsers(roomUsers);
            
            return Status.SuccessfullyCreatedRoom;
        }

        public async Task<string> ActionEnterRoom(Session session, string roomName)
        {
            if (!session.IsUserLoggedIn)
            {
                return Status.UserNotLoggedIn;
            }

            Room room = await _roomService.GetRoom(someRoom => someRoom.RoomName == roomName);
            bool hasEntered = await session.EnterRoom(room);

            if (hasEntered)
            {
                return Status.SuccessfullyEnteredRoom;
            }

            return Status.NoSuchRoomFound;
        }
        
        public string ActionExitRoom(Session session)
        {
            if (session.CurrentRoom == null)
            {
                return Status.NotInTheRoom;
            }

            session.ExitRoom();

            return Status.SuccessfullyExitedRoom;
        }

        public async Task<string> ActionViewRooms(Session session)
        {
            if (session.CurrentUser == null)
            {
                return Status.UserNotLoggedIn;
            }

            IEnumerable<Room> rooms = await _roomUsersService.GetRoomsOfUser(session.CurrentUser);
            
            IList<string> roomNames = new List<string>();

            foreach (var room in rooms)
            {
                roomNames.Add(room.RoomName);
            }

            if (roomNames.Count > 0)
            {
                string names = String.Join("\n", roomNames);
                return names;
            }

            return Status.NoRoomsFound;
        }
// KEKKKEKEKEEKEK
        public async Task<string> ActionViewUsers(Session session)
        {
            if (session.CurrentUser == null)
            {
                return Status.UserNotLoggedIn;
            }

            if (session.CurrentRoom == null)
            {
                return Status.NotInTheRoom;
            }

            IEnumerable<User> users = await _roomUsersService.GetUsersOfRoom(session.CurrentRoom);
            IList<string> userNames = new List<string>();

            foreach (var user in users)
            {
                userNames.Add(user.Nickname);
            }

            string names = String.Join("\n", userNames);

            return names;

        }

        public async Task<string> ActionCreateChat(Session session, string chatName, string isPrivate, int roomId)
        {
            if (session.CurrentUser == null)
            {
                return Status.UserNotLoggedIn;
            }

            if (session.CurrentRoom == null)
            {
                return Status.NotInTheRoom;
            }

            if (String.IsNullOrEmpty(chatName))
            {
                return Status.ChatNameEmpty;
            }

            bool isChatPrivate = isPrivate.Equals("yes") ? true : false;

            Chat chat = new Chat() {Name = chatName, IsPrivate = isChatPrivate, RoomId = roomId};
            bool chatExists = await _chatService.ChatExists(chat, session.CurrentRoom);

            if (chatExists)
            {
                return Status.ChatAlreadyExists;
            }
            
            _chatService.CreateChat(chat, session.CurrentRoom);

            return Status.SuccessfullyCreatedChat;
        }

        public async Task<string> ActionViewChats(Session session)
        {
            if (session.CurrentUser == null)
            {
                return Status.UserNotLoggedIn;
            }
            
            if (session.CurrentRoom == null)
            {
                return Status.NotInTheRoom;
            }

            IEnumerable<Chat> chats = await _chatService.GetChats(session.CurrentRoom);

            IList<string> chatNames = new List<string>();

            foreach (var chat in chats)
            {
                chatNames.Add(chat.Name);
            }

            if (chatNames.Count > 0)
            {
                string names = String.Join("\n", chatNames);
                return names;
            }

            return Status.NoChatsFound;
        }

        public async Task<string> ActionEnterChat(Session session, string chatName)
        {
            if (!session.IsUserLoggedIn)
            {
                return Status.UserNotLoggedIn;
            }
            
            if (session.CurrentRoom == null)
            {
                return Status.NotInTheRoom;
            }

            Chat chat = await _chatService.GetChat(chatName, session.CurrentRoom);
            bool hasEntered = await session.EnterChat(chat);

            if (hasEntered)
            {
                return Status.SuccessfullyEnteredChat;
            }

            return Status.NoSuchChatFound;
        }

        public string ActionExitChat(Session session)
        {
            if (session.CurrentChat == null)
            {
                return Status.NotInTheChat;
            }
            
            if (session.CurrentRoom == null)
            {
                return Status.NotInTheRoom;
            }

            session.ExitChat();

            return Status.SuccessfullyExitedChat;
        }

        public async Task<string> ActionCreateRole(Session session, Room room)
        {
            return "";
        }

        public async Task<Tuple<string, UsersInvitation>> ActionCheckInvitations(Session session)
        {
            if (!session.IsUserLoggedIn)
            {
                return new Tuple<string, UsersInvitation>(Status.UserNotLoggedIn, null);
            }

            var userInvitationsAsync = await _usersInvitationService.GetUsers();
            var usersInvitations = userInvitationsAsync.ToList();
            
            var isUserInvited = usersInvitations.Where(user => _session.CurrentUser.Id == user.UserId).FirstOrDefault();

            var rooms = await _roomService.GetRooms();
            var roomName = rooms.ToList().Where(room => room.Id == isUserInvited.RoomId).FirstOrDefault().RoomName;
            if (isUserInvited != null)
            {
                return new Tuple<string, UsersInvitation>(String.Format(Status.InvitationsFound, roomName), isUserInvited);
            }
            else
            {
                return new Tuple<string, UsersInvitation>(Status.NoInvitationsFound, null);
            }

        }

        public async Task<string> ActionAcceptInvitation(Session session, string answer, int roomId, int userId)
        {
            if (answer.Equals("yes"))
            {
                var roomUsers = new RoomUsers()
                    {RoomId = roomId, UserId = userId};
                _roomUsersService.CreateRoomUsers(roomUsers);
                return Status.InvitationAccepted;
            }
            else
            {
                return Status.InvitationDeclined;
            }
        }

        public async Task<string> ActionInviteUser(Session session, string userName)
        {
            if (!session.IsUserLoggedIn)
            {
                return Status.UserNotLoggedIn;
            }

            if (session.CurrentRoom == null)
            {
                return Status.NotInTheRoom;
            }

            var userToInvite = await _userService.GetUser(user => user.Nickname == userName);

        //    Thread.Sleep(1000);
            
            if (userToInvite is not null)
            {
                _usersInvitationService.AddUser(userToInvite.Id, _session.CurrentRoom.Id);
                _emailService.SendingEmailOnInviting(userToInvite,_session.CurrentRoom.RoomName);
                return Status.SuccessfullyInvitedUser;
            }
            else
            {
                return Status.NoSuchUserExists;
            }
        }
    }
}

        // private void OpenRegisterPage()
        // {
        //     if (!_session.IsUserLoggedIn)
        //     {
        //         Console.Write("Username: ");
        //         string username = Console.ReadLine().Trim();
        //
        //         while (_userService.UserExists(username))
        //         {
        //             Console.WriteLine($"Name {username} is already taken");
        //             Console.Write("Username: ");
        //             username = Console.ReadLine().Trim();
        //         }
        //
        //         //Console.Write("Email: ");
        //         //string email = Console.ReadLine();
        //
        //         //// check if email is correct
        //         //// check if email exists
        //         ///
        //         Console.Write("Email: ");
        //         string email = Console.ReadLine().Trim();
        //
        //         string pageContent;
        //
        //         Console.Write("Password: ");
        //         string password = Console.ReadLine().Trim();
        //
        //         // check if password is good enough
        //
        //         Console.Write("Confirm password: ");
        //         string confirmPassword = Console.ReadLine().Trim();
        //
        //         while (!password.Equals(confirmPassword))
        //         {
        //             Console.WriteLine("Passwords did not match");
        //             Console.Write("Confirm password: ");
        //             confirmPassword = Console.ReadLine().Trim();
        //         }
        //
        //         // hash password
        //
        //         User user = new User {Nickname = username, Password = password, Email = email};
        //         _userService.CreateUser(user);
        //         _session.TryLogin(user.Nickname, user.Password);
        //
        //         // create register class
        //
        //         if (_session.IsUserLoggedIn)
        //         {
        //             _emailService.SendingEmailOnRegistration(user);
        //             Console.WriteLine("Successfully logged in!\n");
        //             pageContent = string.Concat(
        //                 "Go to:\n\n",
        //                 "main\n",
        //                 "logout\n",
        //                 "exit\n"
        //             );
        //         }
        //         else
        //         {
        //             pageContent = string.Concat(
        //                 "Go to:\n\n",
        //                 "login\n",
        //                 "register\n",
        //                 "exit\n"
        //             );
        //         }
        //
        //         Console.WriteLine(pageContent);
        //     }
        //
        // }