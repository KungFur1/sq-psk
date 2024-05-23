using System.Net;
using MassTransit;
using MongoDB.Driver;
using MongoDB.Entities;
using Polly;
using Polly.Extensions.Http;
using RecipesSearchService.Consumers;
using RecipesSearchService.Data;
using RecipesSearchService.Models;
using RecipesSearchService.Services;
using ZstdSharp.Unsafe;

var builder = WebApplication.CreateBuilder(args);

// DI Components
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<RecipesServiceHttpClient>().AddPolicyHandler(GetPolicy());
builder.Services.AddMassTransit(x => 
{
    x.AddConsumersFromNamespaceContaining<RecipeCreatedConsumer>();

    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("recipes-search", false));

    x.UsingRabbitMq((context, cfg) => 
    {
        cfg.ReceiveEndpoint("recipes-search-recipe-created", e =>
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


app.UseAuthorization();
app.MapControllers();
app.Lifetime.ApplicationStarted.Register(async () => {
try
{
    await DbInitializer.InitializeDb(app);
}
catch (Exception e)
{
    Console.WriteLine(e);
}
});


app.Run();

static IAsyncPolicy<HttpResponseMessage> GetPolicy()
    => HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(5));