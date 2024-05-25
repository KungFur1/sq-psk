using ImagesService;
using ImagesService.Consumers;
using ImagesService.EndpointHelpers;
using MassTransit;
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
builder.Services.AddMassTransit(x => 
{
    x.AddConsumersFromNamespaceContaining<RecipeCreatedConsumer>();

    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("images", false));

    x.UsingRabbitMq((context, cfg) => 
    {
        cfg.ReceiveEndpoint("images-endpoint", e =>
        {
            e.UseMessageRetry(r => r.Interval(60, 8));

            e.ConfigureConsumer<RecipeCreatedConsumer>(context);
            e.ConfigureConsumer<RecipeUpdatedConsumer>(context);
            e.ConfigureConsumer<RecipeDeletedConsumer>(context);
        });

        cfg.ConfigureEndpoints(context);
    });
});

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