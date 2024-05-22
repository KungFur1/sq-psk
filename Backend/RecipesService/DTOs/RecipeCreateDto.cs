using System.ComponentModel.DataAnnotations;

namespace RecipesService;

public class RecipeCreateDto
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string ShortDescription { get; set; }
    [Required]
    public string IngredientsList { get; set; }
    [Required]
    public string CookingSteps { get; set; }
    [Required]
    public string ImageUrl { get; set; }
}
