using System.ComponentModel.DataAnnotations;

namespace RecipesService;

public class ReviewCreateDto
{
    //[Range(1, 5, ErrorMessage = "Star rating must be between 1 and 5")]
    public int StarRating { get; set; }
    public string Comment { get; set; }
    public Guid RecipeId { get; set; }
}
