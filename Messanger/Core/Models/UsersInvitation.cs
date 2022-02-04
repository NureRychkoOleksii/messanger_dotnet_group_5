namespace Core.Models
{
    public class UsersInvitation : IdKey
    {
        public User User { get; set; }
        
        public int UserId { get; set; }
        
        public Room Room { get; set; }
        
        public int RoomId { get; set; }
        
    }
}