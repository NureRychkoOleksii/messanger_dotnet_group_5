using System.Collections.Generic;

namespace Core.Models
{
    public class RoomUsers
    {
        public Room Room { get; set; }
        public List<User> Users { get; set; }
    }
}