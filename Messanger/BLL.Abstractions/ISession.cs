using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Core.Models;

namespace BLL.Abstractions
{
    public interface ISession
    {
        public Task<bool> TryLogin(string username, string password);
        public void TryLogout();
        public Task<bool> EnterRoom(Room room);
        public bool ExitRoom();
        Task<bool> EnterChat(Chat chatToEnter);
        public bool ExitChat();
    }
}