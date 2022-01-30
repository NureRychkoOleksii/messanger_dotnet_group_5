using System.Collections.Generic;
using Core;
using Core.Models;

namespace BLL.Abstractions.Interfaces
{
    public interface IChatService
    {
        bool CreateChat(Chat chat, Room room);
        void DeleteChat(Chat chat);
        void UpdateChat(Chat chat);
        IEnumerable<Chat> GetChats(Room room);
        public bool ChatExists(Chat chat, Room room);
        public Chat GetChat(string chatName, Room room);
    }
}