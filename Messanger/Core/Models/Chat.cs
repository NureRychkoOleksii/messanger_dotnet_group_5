namespace Core.Models
{
    public class Chat : IdKey
    {
        public string Name { get; set; }
        
        public RoomUsers RoomUsers { get; set; }
        
        public bool IsPrivate { get; set; }
    }
}