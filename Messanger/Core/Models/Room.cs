using System.Collections.Generic;
using Core.Models;

namespace Core
{
    public class Room : IdKey
    {
        public string RoomName { get; set; }

        public Dictionary<int, Role> Roles = new Dictionary<int, Role>()
        {
            [0] = new Role()
            {
                RoleName = "Admin", Permissions = new Dictionary<string, bool>()
                {
                    ["Manage roles"] = true,
                    ["Rename room"] = true
                }
            },
            [1] = new Role() {RoleName = "User"}
        };

        // public List<Role> Roles { get; set; }
        //
        // public List<Chat> Chats { get; set; }
    }
}