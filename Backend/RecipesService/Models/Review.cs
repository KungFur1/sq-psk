using RecipesService.Models;

namespace RecipesService;

public class Review
{
    public Guid Id { get; set; }
    public int StarRating { get; set; }
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public Guid UserId { get; set; }
    public Guid RecipeId { get; set; }

    public Recipe Recipe { get; set; }
}
