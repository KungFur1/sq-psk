using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;
using RecipesSearchService.Models;
using RecipesSearchService.Services;

namespace RecipesSearchService.Data;

public static class DbInitializer
{
    public static async Task InitializeDb(WebApplication app)
    {
        Console.WriteLine("DbInitializer: initializing and indexing...");
        await DB.InitAsync("RecipesSearchDatabase", MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("DockerMongoDbConnectionString")));
        await DB.Index<RecipeSearchItem>()
            .Key(x => x.Title, KeyType.Text)
            .Key(x => x.ShortDescription, KeyType.Text)
            .Key(x => x.IngredientsList, KeyType.Text)
            .Key(x => x.CookingSteps, KeyType.Text)
            .CreateAsync();

        var count = await DB.CountAsync<RecipeSearchItem>();

        using var scope = app.Services.CreateScope();
        var httpClient = scope.ServiceProvider.GetRequiredService<RecipesServiceHttpClient>();

        Console.WriteLine("DbInitializer: calling get missing from recipes service...");
        var missingRecipeSearchItems = await httpClient.GetMissingFromRecipesService();
        Console.WriteLine($"DbInitializer: {missingRecipeSearchItems.Count} items returned from recipes service...");

        if (missingRecipeSearchItems.Count > 0) await DB.SaveAsync(missingRecipeSearchItems);
    }
}
