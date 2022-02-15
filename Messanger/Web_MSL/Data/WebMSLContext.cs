using Microsoft.EntityFrameworkCore;
using Web_MSL.Models;

namespace Web_MSL.Data;

public class WebMSLContext : DbContext
{
    public WebMSLContext(DbContextOptions<WebMSLContext> options) : base(options)
    {
        
    }

    public WebMSLContext()
    {
        
    }

    private DbSet<User> Users { get; set; }
}