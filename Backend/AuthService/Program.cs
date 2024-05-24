using AuthService;
using AuthService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DI components
builder.Services.AddControllers();
builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DockerPostgreSQLConnectionString"));
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();


// Midleware
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
try{
    DbInitializer.InitializeDb(app);
}
catch(Exception e)
{
    Console.WriteLine(e.Message);
}

app.Run();