namespace Core.Models
{
    public class UsersInvitation : IdKey
    {
        public int UserId { get; set; }
        
        public int RoomId { get; set; }
        
    }
}