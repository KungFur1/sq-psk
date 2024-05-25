using Microsoft.EntityFrameworkCore;

namespace ImagesService;

public static class DbInitializer
{
    public static void InitializeDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var imagesDbContext = scope.ServiceProvider.GetService<ImagesDbContext>();

        Console.WriteLine("DbInitializer: applying migrations...");
        imagesDbContext.Database.Migrate();
    }
}
