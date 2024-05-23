using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Entities;
using RecipesSearchService.Models;

namespace RecipesSearchService.Consumers;

public class RecipeUpdatedConsumer : IConsumer<RecipeUpdated>
{
public async Task Consume(ConsumeContext<RecipeUpdated> consumable)
    {
        Console.WriteLine("--> Consuming RecipeUpdated id: " + consumable.Message.Id);

        var dbRecipeSearchItem = await DB.Find<RecipeSearchItem>().OneAsync(consumable.Message.Id);
        if (dbRecipeSearchItem == null) throw new Exception("During consuming of RecipeUpdated, item with such id not found");

        dbRecipeSearchItem.Title = consumable.Message.Title ?? dbRecipeSearchItem.Title;
        dbRecipeSearchItem.ShortDescription = consumable.Message.ShortDescription ?? dbRecipeSearchItem.ShortDescription;
        dbRecipeSearchItem.IngredientsList = consumable.Message.IngredientsList ?? dbRecipeSearchItem.IngredientsList;
        dbRecipeSearchItem.CookingSteps = consumable.Message.CookingSteps ?? dbRecipeSearchItem.CookingSteps;
        dbRecipeSearchItem.ImageUrl = consumable.Message.ImageUrl ?? dbRecipeSearchItem.ImageUrl;

        await DB.Update<RecipeSearchItem>()
            .MatchID(dbRecipeSearchItem.ID)
            .ModifyWith(dbRecipeSearchItem)
            .ExecuteAsync();
    }
}
