namespace RecipesService;

public class ReviewResponseDto
{
    public Guid Id { get; set; }
    public int StarRating { get; set; }
    public string Comment { get; set; }
    public Guid UserId { get; set; }
    public Guid RecipeId { get; set; }
}
