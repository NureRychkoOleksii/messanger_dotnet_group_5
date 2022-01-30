using System.Collections.Generic;
using System.Linq;
using BLL.Abstractions.Interfaces;
using Core;
using Core.Models;
using DAL.Abstractions.Interfaces;

namespace BLL.Services
{
    public class ChatService : IChatService
    {
        private readonly IRepository<Chat> _repository;
        private readonly IRoomService _roomService;

        public ChatService(IRepository<Chat> repository, IRoomService roomService)
        {
            _repository = repository;
            _roomService = roomService;
        }
        
        public bool CreateChat(Chat chat, Room room)
        {
            if (!this.ChatExists(chat, room))
            {
                _repository.CreateObjectAsync(chat);
                
                room.Chats.Add(chat);
                _roomService.UpdateRoom(room);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DeleteChat(Chat chat)
        {
            _repository.DeleteObjectAsync(chat);
        }

        public void UpdateChat(Chat chat)
        {
            _repository.UpdateObjectAsync(chat);
        }

        public IEnumerable<Chat> GetChats(Room room)
        {
            var chats = _repository.GetAllAsync(typeof(Chat)).Result
                .Where(chat => chat.RoomId == room.Id);
            return chats;
        }

        public bool ChatExists(Chat chat, Room room)
        {
            var roomChats = this.GetChats(room);

            var searchedChat = roomChats.Where(roomChat => roomChat.Name == chat.Name).FirstOrDefault();

            if (searchedChat != null)
            {
                return true;
            }

            return false;
        }

        public Chat GetChat(string chatName, Room room)
        {
            var chats = this.GetChats(room)
                .Where(chat => chat.Name == chatName)
                .FirstOrDefault();
            return chats;
        }
    }
}