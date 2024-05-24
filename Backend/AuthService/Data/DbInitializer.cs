
using AuthService.EndpointHelpers;
using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data;

public static class DbInitializer
{
    public static void InitializeDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        SeedData(scope.ServiceProvider.GetService<AuthDbContext>());
    }

    private static void SeedData(AuthDbContext authDbContext)
    {
        Console.WriteLine("DbInitializer: applying migrations...");
        authDbContext.Database.Migrate();

        if (authDbContext.Users.Any())
        {
            Console.WriteLine("DbInitializer: data present, no need to seed...");
            return;
        }

        Console.WriteLine("DbInitializer: adding seed data...");
        var users = new List<User>()
        {
            new User {
                Id = Guid.Parse("f1819dad-76e3-41b3-9a76-e7fa6b974cc4"),
                Email = "easy-life@gmail.com",
                HashedPassword = PasswordHasher.HashPassword("easy-life")
            },

            new User {
                Id = Guid.Parse("cb8c4501-be84-4377-98a9-f40de26f1f84"),
                Email = "pyragas@gmail.com",
                HashedPassword = PasswordHasher.HashPassword("obuolys")
            }
        };

        authDbContext.AddRange(users);
        authDbContext.SaveChanges();
        Console.WriteLine("DbInitializer: seed data added");
    }
}
