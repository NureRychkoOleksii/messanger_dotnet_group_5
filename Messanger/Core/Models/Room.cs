using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Core.Models;

namespace Core
{
    public class Room : IdKey
    {
        public string RoomName { get; set; }

        private Role Admin = new Role()
        {
            RoleName = "Admin",
            ManageRoles = true, RenameRoom = true, Id = 0
        };

        private Role User = new Role()
        {
            RoleName = "User", Id = 1
        };

        public List<Chat> Chats = new List<Chat>() {};

        // public List<Role> Roles { get; set; }
        //
        // public List<Chat> Chats { get; set; }
    }
}