using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipesService.Data;
using RecipesService.DTOs;
using RecipesService.EndpointHelpers;
using RecipesService.Models;

namespace RecipesService;

[ApiController]
[Route("api/recipes")]
public class RecipesController : ControllerBase
{
    private readonly RecipesDbContext _recipesDbContext;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public RecipesController(RecipesDbContext recipesDbContext, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _recipesDbContext = recipesDbContext;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<ActionResult<List<RecipeResponseDto>>> GetAllRecipes(string date)
    {
        var query = _recipesDbContext.Recipes
            .Include(r => r.Reviews)
            .OrderBy(x => x.Title)
            .AsQueryable();

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
            .Include(r => r.Reviews)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (recipe == null) return NotFound();
        
        return _mapper.Map<RecipeResponseDto>(recipe);
    }

    [HttpPost]
    [Authenticate]
    public async Task<ActionResult<RecipeResponseDto>> CreateRecipe(
        [FromBody] RecipeCreateDto recipeCreateDto, 
        [ModelBinder(BinderType = typeof(SessionInfoModelBinder))] SessionInfoDto sessionInfo)
    {
        var recipe = _mapper.Map<Recipe>(recipeCreateDto);
        recipe.UserId = sessionInfo.UserId;

        _recipesDbContext.Recipes.Add(recipe);
        var newRecipe = _mapper.Map<RecipeResponseDto>(recipe);
        await _publishEndpoint.Publish(_mapper.Map<RecipeCreated>(newRecipe));
        bool succesful = await _recipesDbContext.SaveChangesAsync() > 0;

        if (!succesful) return StatusCode(500, "Database operation failed");

        return CreatedAtAction(nameof(GetRecipeById), new {recipe.Id}, newRecipe);
    }

    [HttpPut]
    [Route("{id}")]
    [Authenticate]
    public async Task<ActionResult<RecipeResponseDto>> UpdateRecipe(
        Guid id,
        [FromBody] RecipeUpdateDto recipeUpdateDto,
        [ModelBinder(BinderType = typeof(SessionInfoModelBinder))] SessionInfoDto sessionInfo)
    {
        var recipe = await _recipesDbContext.Recipes
            .FirstOrDefaultAsync(x => x.Id == id);
        
        if (recipe == null) return NotFound();

        if (recipe.UserId != sessionInfo.UserId) return Unauthorized();

        recipe.Title = recipeUpdateDto.Title ?? recipe.Title;
        recipe.ShortDescription = recipeUpdateDto.ShortDescription ?? recipe.ShortDescription;
        recipe.IngredientsList = recipeUpdateDto.IngredientsList ?? recipe.IngredientsList;
        recipe.CookingSteps = recipeUpdateDto.CookingSteps ?? recipe.CookingSteps;
        recipe.ImageId = recipeUpdateDto.ImageId ?? recipe.ImageId;
        recipe.PrepTime = recipeUpdateDto.PrepTime ?? recipe.PrepTime;
        recipe.CookTime = recipeUpdateDto.CookTime ?? recipe.CookTime;
        recipe.Servings = recipeUpdateDto.Servings ?? recipe.Servings;
        recipe.UpdatedAt = DateTime.UtcNow;

        await _publishEndpoint.Publish(_mapper.Map<RecipeUpdated>(recipe));
        bool succesful = await _recipesDbContext.SaveChangesAsync() > 0;

        if (!succesful) return StatusCode(500, "Database operation failed");
        
        return _mapper.Map<RecipeResponseDto>(recipe);
    }

    [HttpDelete]
    [Route("{id}")]
    [Authenticate]
    public async Task<ActionResult> DeleteRecipeById(
        Guid id,
        [ModelBinder(BinderType = typeof(SessionInfoModelBinder))] SessionInfoDto sessionInfo)
    {
        var recipe = await _recipesDbContext.Recipes
            .FindAsync(id);
        
        if (recipe == null) return NotFound();

        if (recipe.UserId != sessionInfo.UserId) return Unauthorized();

        _recipesDbContext.Recipes.Remove(recipe);
        await _publishEndpoint.Publish(_mapper.Map<RecipeDeleted>(new RecipeDeleted { Id = recipe.Id }));
        var succesful = await _recipesDbContext.SaveChangesAsync() > 0;

        if (!succesful) return StatusCode(500, "Database operation failed");

        return NoContent();
    }
}
