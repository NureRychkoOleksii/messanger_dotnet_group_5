using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Core.Models;

namespace BLL.Abstractions.Interfaces
{
    public interface IChatService
    {
        void CreateChat(Chat chat, Room room);
        void DeleteChat(Chat chat);
        void UpdateChat(Chat chat);
        Task<IEnumerable<Chat>> GetChats(Room room);
        public Task<bool> ChatExists(Chat chat, Room room);
        Task<Chat> GetChat(string chatName, Room room);
    }
}