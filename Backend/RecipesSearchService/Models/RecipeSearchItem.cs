using MongoDB.Entities;

namespace RecipesSearchService.Models;

public class RecipeSearchItem : Entity
{
    // by default id becomes the id of a mongodb entry
    public string UserId { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string IngredientsList { get; set; }
    public string CookingSteps { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
