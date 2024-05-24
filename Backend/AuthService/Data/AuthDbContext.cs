using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set;}
    public DbSet<Session> Sessions { get; set; }
}
