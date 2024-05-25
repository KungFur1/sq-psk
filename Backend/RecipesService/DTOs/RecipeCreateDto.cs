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
    public Guid ImageId { get; set; }
    [Required]
    public int PrepTime { get; set; }
    [Required]
    public int CookTime { get; set; }
    [Required]
    public int Servings { get; set; }
}
