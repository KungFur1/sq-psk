namespace Contracts;

public class RecipeUpdated
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string IngredientsList { get; set; }
    public string CookingSteps { get; set; }
    public Guid? ImageId { get; set; }
    public int? PrepTime { get; set; }
    public int? CookTime { get; set; }
    public int? Servings { get; set; }
}
