using System.Collections.Generic;
using Core.Models;

namespace Core
{
    public class Room : IdKey
    {
        public string RoomName { get; set; }
        
        // public List<Role> Roles { get; set; }
        //
        // public List<Chat> Chats { get; set; }
    }
}