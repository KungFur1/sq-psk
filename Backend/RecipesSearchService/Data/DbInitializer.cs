using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;
using RecipesSearchService.Models;

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

        if (count != 0)
        {
            Console.WriteLine("DbInitializer: data present, no need to seed...");
            return;
        }
        else
        {
            Console.WriteLine("DbInitializer: no data present, attempting to seed...");

            var itemData = await File.ReadAllTextAsync("Data/recipes.json");
            var options = new JsonSerializerOptions{PropertyNameCaseInsensitive=true};
            var items = JsonSerializer.Deserialize<List<RecipeSearchItem>>(itemData, options);
            await DB.SaveAsync(items);

            Console.WriteLine("DbInitializer: added seed data...");
        }
    }
}
