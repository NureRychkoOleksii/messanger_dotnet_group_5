using System.ComponentModel.DataAnnotations;

namespace Web_MSL.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    
    public string Nickname { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
}