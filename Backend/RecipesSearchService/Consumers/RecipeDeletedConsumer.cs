using Contracts;
using MassTransit;
using MongoDB.Entities;
using RecipesSearchService.Models;

namespace RecipesSearchService.Consumers;

public class RecipeDeletedConsumer : IConsumer<RecipeDeleted>
{
    public async Task Consume(ConsumeContext<RecipeDeleted> consumable)
    {
        Console.WriteLine("--> Consuming RecipeDeleted id: " + consumable.Message.Id);

        await DB.DeleteAsync<RecipeSearchItem>(consumable.Message.Id);
    }
}
