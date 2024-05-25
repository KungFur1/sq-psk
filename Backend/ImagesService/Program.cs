using ImagesService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DI components
builder.Services.AddControllers();
builder.Services.AddDbContext<ImagesDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DockerPostgreSQLConnectionString"));
});

var app = builder.Build();


// Middleware
app.MapControllers();
try{
    DbInitializer.InitializeDb(app);
}
catch(Exception e)
{
    Console.WriteLine(e.Message);
}

app.Run();