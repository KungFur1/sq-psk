namespace Contracts;

public class RecipeUpdated
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string IngredientsList { get; set; }
    public string CookingSteps { get; set; }
    public string ImageUrl { get; set; }
}
