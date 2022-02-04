using System.Collections.Generic;

namespace Core.Models
{
    public class RoomUsers : IdKey
    {
        public Room Room { get; set; }
        
        public int RoomId { get; set; }
        
        public User User { get; set; }
        public int UserId { get; set; }
        public int UserRole { get; set; }
    }
}