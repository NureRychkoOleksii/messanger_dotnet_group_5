using System.Collections.Generic;

namespace Core.Models
{
    public class RoomUsers : IdKey
    {
        public int RoomId { get; set; }
        public int UserId { get; set; }
        
        public int UserRole { get; set; }
    }
}