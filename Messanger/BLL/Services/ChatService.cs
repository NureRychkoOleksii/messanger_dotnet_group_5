using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        
        public async void CreateChat(Chat chat, Room room)
        {
            await Task.Run(() => _repository.CreateObjectAsync(chat));
            room.Chats.Add(chat);
            _roomService.UpdateRoom(room);
        }

        public void DeleteChat(Chat chat)
        {
            _repository.DeleteObjectAsync(chat);
        }

        public void UpdateChat(Chat chat)
        {
            _repository.UpdateObjectAsync(chat);
        }

        public async Task<IEnumerable<Chat>> GetChats(Room room)
        {
            var chats = await _repository.GetAllAsync(typeof(Chat));
            var roomChats = chats.Where(chat => chat.RoomId == room.Id);
            return roomChats;
        }

        public async Task<bool> ChatExists(Chat chat, Room room)
        {
            var roomChats = await this.GetChats(room);

            var searchedChat = roomChats.Where(roomChat => roomChat.Name == chat.Name).FirstOrDefault();

            if (searchedChat != null)
            {
                return true;
            }

            return false;
        }

        public async Task<Chat> GetChat(string chatName, Room room)
        {
            var chats = await this.GetChats(room);
            var chat = chats.Where(chat => chat.Name == chatName)
                .FirstOrDefault();
            return chat;
        }
    }
}