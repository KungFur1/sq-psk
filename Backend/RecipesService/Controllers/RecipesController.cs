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
    [Authenticate]
    public async Task<ActionResult<RecipeResponseDto>> CreateRecipe(RecipeCreateDto recipeCreateDto)
    {
        // TEST-------------
        var sessionInfo = HttpContext.Items["SessionInfo"] as SessionInfoDto;
        if (sessionInfo == null)
        {
            System.Console.WriteLine("------->Is null");
        }
        System.Console.WriteLine("---------> User Id: " + sessionInfo.UserId);
        // TEST-------------

        var recipe = _mapper.Map<Recipe>(recipeCreateDto);
        // TODO: Replace UserId with the logged in user id from the JWT
        recipe.UserId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");

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
        recipe.UpdatedAt = DateTime.UtcNow;

        await _publishEndpoint.Publish(_mapper.Map<RecipeUpdated>(recipe));
        bool succesful = await _recipesDbContext.SaveChangesAsync() > 0;

        if (!succesful) return StatusCode(500, "Database operation failed");
        
        return _mapper.Map<RecipeResponseDto>(recipe);
    }

    [HttpDelete]
    [Route("{id}")]
    [Authenticate]
    public async Task<ActionResult> DeleteRecipeById(Guid id)
    {
        var recipe = await _recipesDbContext.Recipes
            .FindAsync(id);
        
        if (recipe == null) return NotFound();

        // TODO: check if the recipe user id == logged in user id

        _recipesDbContext.Recipes.Remove(recipe);
        await _publishEndpoint.Publish(_mapper.Map<RecipeDeleted>(new RecipeDeleted { Id = recipe.Id.ToString() }));
        var succesful = await _recipesDbContext.SaveChangesAsync() > 0;

        if (!succesful) return StatusCode(500, "Database operation failed");

        return NoContent();
    }
}
