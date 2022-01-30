using System;
using System.Linq;
using Core;
using Core.Models;
using BLL.Abstractions.Interfaces;

namespace BLL
{
    public class Session
    {
        private readonly IUserService _userService;
        private readonly IRoomService _roomService;
        private readonly IRoomUsersService _roomUsersService;
        private readonly IChatService _chatService;

        private bool _isUserLoggedIn = false;
        private User _currentUser;
        private Room _currentRoom;
        private Chat _currentChat;

        public Session(IUserService userService, IRoomUsersService roomUsersService, IRoomService roomService,
            IChatService chatService)
        {
            _userService = userService;
            _roomService = roomService;
            _roomUsersService = roomUsersService;
            _chatService = chatService;
        }

        public bool IsUserLoggedIn => _isUserLoggedIn;
        public User CurrentUser => _currentUser;
        public Room CurrentRoom => _currentRoom;
        public Chat CurrentChat => _currentChat;
        

        public bool TryLogin(string username, string password)
        {
            var users = _userService.GetUsers();
            var user = users.Where(x => x.Nickname == username).FirstOrDefault();

            if (user != null && user.Password == password)
            {
                _isUserLoggedIn = true;
                _currentUser = user;
                return true;
            }

            return false;
        }

        public void TryLogout()
        {
            if (_isUserLoggedIn && _currentUser != null)
            {
                _isUserLoggedIn = false;
                _currentUser = null;
            }
        }

        public bool EnterRoom(Room room)
        {
            if (room == null)
            {
                return false;
            }
            var roomUsers = _roomUsersService.GetRoomsOfUser(_currentUser)
                .Where(roomUser => roomUser.Id == room.Id)
                .FirstOrDefault();
            if (roomUsers != null)
            {
                _currentRoom = room;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ExitRoom()
        {
            if (_currentRoom != null)
            {
                _currentRoom = null;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EnterChat(Chat chatToEnter)
        {
            if (chatToEnter == null)
            {
                return false;
            }

            var chats = _chatService.GetChats(_currentRoom)
                .Where(chat => chat.Name == chatToEnter.Name)
                .FirstOrDefault();

            if (chats != null)
            {
                _currentChat = chatToEnter;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ExitChat()
        {
            if (_currentChat != null)
            {
                _currentChat = null;
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }
}