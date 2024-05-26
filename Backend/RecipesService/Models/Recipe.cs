namespace RecipesService.Models;

public class Recipe
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string IngredientsList { get; set; }
    public string CookingSteps { get; set; }
    public Guid ImageId { get; set; }
    public int PrepTime { get; set; }
    public int CookTime { get; set; }
    public int Servings { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Review> Reviews{ get; set; } = new List<Review>();
}
