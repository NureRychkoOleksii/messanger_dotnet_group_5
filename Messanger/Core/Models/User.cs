using System.Collections.Generic;
using Core.Models;

namespace Core
{
    public class User : IdKey
    {
        public string Nickname { get; set; }
        
        public string Password { get; set; }
        
        public string Email { get; set; }
        
        // public List<Room> Room { get; set; }
        //
        // public List<Role> Roles { get; set; }
    }
}