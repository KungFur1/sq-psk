namespace RecipesService;

public class RecipeUpdateDto
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string IngredientsList { get; set; }
    public string CookingSteps { get; set; }
    public string ImageUrl { get; set; }
}
