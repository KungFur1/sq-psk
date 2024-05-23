using MongoDB.Entities;
using RecipesSearchService.Models;

namespace RecipesSearchService.Services;

public class RecipesServiceHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public RecipesServiceHttpClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    // Will always return at least one because mongoDb cuts of end of date, so UpdatedAt is always slightly lower in mongoDb than in postgreSql
    public async Task<List<RecipeSearchItem>> GetMissingFromRecipesService()
    {
        var lastUpdated = await DB.Find<RecipeSearchItem, string>()
            .Sort(x => x.Descending(a => a.UpdatedAt))
            .Project(x => x.UpdatedAt.ToString())
            .ExecuteFirstAsync();
        
        Console.WriteLine($"RecipesServiceHttpClient: fetching recipes from recipes service, date={lastUpdated}...");
        return await _httpClient.GetFromJsonAsync<List<RecipeSearchItem>>(_configuration["RecipesServiceUrl"] + "/api/recipes?date=" + lastUpdated);
    }
}
