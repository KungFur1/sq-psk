using Microsoft.EntityFrameworkCore;
using RecipesService;
using RecipesService.Data;

var builder = WebApplication.CreateBuilder(args);


// DI components
builder.Services.AddControllers();
builder.Services.AddDbContext<RecipesDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DockerPostgreSQLConnectionString"));
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();


app.UseAuthorization();
app.MapControllers();
try{
    DbInitializer.InitializeDb(app);
}
catch(Exception e)
{
    Console.WriteLine(e.Message);
}

app.Run();