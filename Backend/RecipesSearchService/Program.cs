using MongoDB.Driver;
using MongoDB.Entities;
using RecipesSearchService.Data;
using RecipesSearchService.Models;

var builder = WebApplication.CreateBuilder(args);

// DI Components
builder.Services.AddControllers();


var app = builder.Build();


app.UseAuthorization();
app.MapControllers();
try
{
    await DbInitializer.InitializeDb(app);
}
catch (Exception e)
{
    Console.WriteLine(e);
}

app.Run();