using Microsoft.EntityFrameworkCore;
using RecipesService;
using RecipesService.Data;
using MassTransit;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);


// DI components
builder.Services.AddControllers();
builder.Services.AddDbContext<RecipesDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DockerPostgreSQLConnectionString"));
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMassTransit(x => 
{
    x.AddEntityFrameworkOutbox<RecipesDbContext>(o => 
    {
        o.QueryDelay = TimeSpan.FromSeconds(10);
        o.UsePostgres();
        o.UseBusOutbox();
    });

    x.UsingRabbitMq((context, cfg) => 
    {
        cfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddHttpClient();


var app = builder.Build();

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