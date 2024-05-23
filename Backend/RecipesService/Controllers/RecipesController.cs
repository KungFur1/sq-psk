using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipesService.Data;
using RecipesService.DTOs;
using RecipesService.Models;

namespace RecipesService;

[ApiController]
[Route("api/recipes")]
public class RecipesController : ControllerBase
{
    private readonly RecipesDbContext _recipesDbContext;
    private readonly IMapper _mapper;

    public RecipesController(RecipesDbContext recipesDbContext, IMapper mapper)
    {
        _recipesDbContext = recipesDbContext;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<RecipeResponseDto>>> GetAllRecipes(string date)
    {
        var query = _recipesDbContext.Recipes.OrderBy(x => x.Title).AsQueryable();

        if (!string.IsNullOrEmpty(date))
        {
            query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
        }

        return await query.ProjectTo<RecipeResponseDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<RecipeResponseDto>> GetRecipeById(Guid id)
    {
        var recipe = await _recipesDbContext.Recipes
            .FirstOrDefaultAsync(x => x.Id == id);

        if (recipe == null) return NotFound();
        
        return _mapper.Map<RecipeResponseDto>(recipe);
    }

    [HttpPost]
    public async Task<ActionResult<RecipeResponseDto>> CreateRecipe(RecipeCreateDto recipeCreateDto)
    {
        var recipe = _mapper.Map<Recipe>(recipeCreateDto);
        // TODO: Replace UserId with the logged in user id from the JWT
        recipe.UserId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");

        _recipesDbContext.Add(recipe);
        bool succesful = await _recipesDbContext.SaveChangesAsync() > 0;

        if (!succesful) return StatusCode(500, "Database operation failed");

        return CreatedAtAction(nameof(GetRecipeById), new {recipe.Id}, _mapper.Map<RecipeResponseDto>(recipe));
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult<RecipeResponseDto>> UpdateRecipe(Guid id, RecipeUpdateDto recipeUpdateDto)
    {
        var recipe = await _recipesDbContext.Recipes
            .FirstOrDefaultAsync(x => x.Id == id);
        
        if (recipe == null) return NotFound();

        // TODO: check if the logged in user id matches the recipe user id

        recipe.Title = recipeUpdateDto.Title ?? recipe.Title;
        recipe.ShortDescription = recipeUpdateDto.ShortDescription ?? recipe.ShortDescription;
        recipe.IngredientsList = recipeUpdateDto.IngredientsList ?? recipe.IngredientsList;
        recipe.CookingSteps = recipeUpdateDto.CookingSteps ?? recipe.CookingSteps;
        recipe.ImageUrl = recipeUpdateDto.ImageUrl ?? recipe.ImageUrl;

        bool succesful = await _recipesDbContext.SaveChangesAsync() > 0;

        if (!succesful) return StatusCode(500, "Database operation failed");
        
        return _mapper.Map<RecipeResponseDto>(recipe);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> DeleteRecipeById(Guid id)
    {
        var recipe = await _recipesDbContext.Recipes
            .FindAsync(id);
        
        if (recipe == null) return NotFound();

        // TODO: check if the recipe user id == logged in user id

        _recipesDbContext.Remove(recipe);
        var succesful = await _recipesDbContext.SaveChangesAsync() > 0;

        if (!succesful) return StatusCode(500, "Database operation failed");

        return NoContent();
    }
}
