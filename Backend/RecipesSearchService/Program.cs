using System.Net;
using MongoDB.Driver;
using MongoDB.Entities;
using Polly;
using Polly.Extensions.Http;
using RecipesSearchService.Data;
using RecipesSearchService.Models;
using RecipesSearchService.Services;
using ZstdSharp.Unsafe;

var builder = WebApplication.CreateBuilder(args);

// DI Components
builder.Services.AddControllers();
builder.Services.AddHttpClient<RecipesServiceHttpClient>().AddPolicyHandler(GetPolicy());


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