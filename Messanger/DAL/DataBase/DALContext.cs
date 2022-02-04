using Core;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.DataBase;

public class DALContext : DbContext
{

    public DbSet<User> Users { get; set; }
    
    public DbSet<Room> Rooms { get; set; }
    
    public DbSet<Chat> Chats { get; set; }
    
    public DbSet<Role> Roles { get; set; }
    
    public DbSet<RoomUsers> RoomUsers { get; set; }
    
    public DbSet<UsersInvitation> UsersInvitations { get; set; }

    public DALContext(DbContextOptions<DALContext> options):base()
    {
        
    }

    public DALContext()
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=DESKTOP-MPQ8K2S;Database=Messenger_Group5;Trusted_Connection=True;");
    }
    
    
}