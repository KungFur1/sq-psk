using ImagesService;
using ImagesService.EndpointHelpers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DI components
builder.Services.AddControllers(options => 
{
    options.ModelBinderProviders.Insert(0, new SessionInfoModelBinderProvider());
});
builder.Services.AddDbContext<ImagesDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DockerPostgreSQLConnectionString"));
});
builder.Services.AddHttpClient();


var app = builder.Build();


// Middleware
app.MapControllers();
app.UseMiddleware<RemoteAuthHandler>();
try{
    DbInitializer.InitializeDb(app);
}
catch(Exception e)
{
    Console.WriteLine(e.Message);
}

app.Run();