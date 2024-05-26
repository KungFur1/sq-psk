using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipesService.Data;
using RecipesService.EndpointHelpers;

namespace RecipesService;

[ApiController]
[Route("api/reviews")]
public class ReviewsController : ControllerBase
{
    private readonly RecipesDbContext _recipesDbContext;
    private readonly IMapper _mapper;
    public ReviewsController(RecipesDbContext recipesDbContext, IMapper mapper)
    {
        _recipesDbContext = recipesDbContext;
        _mapper = mapper;
    }

    [HttpPost]
    [Authenticate]
    public async Task<ActionResult<ReviewResponseDto>> CreateReview(
        [FromBody] ReviewCreateDto reviewCreateDto,
        [ModelBinder(BinderType = typeof(SessionInfoModelBinder))] SessionInfoDto sessionInfo)
    {   
        var recipe = await _recipesDbContext.Recipes.FirstOrDefaultAsync(x => x.Id == reviewCreateDto.RecipeId);
        if (recipe == null) return NotFound();
        if (recipe.UserId == sessionInfo.UserId) Console.WriteLine("ReviewsController.CreateReview: recipe and review owner user matches");
        var existingReview = await _recipesDbContext.Reviews.FirstOrDefaultAsync(x => x.UserId == sessionInfo.UserId);
        if (existingReview != null) Console.WriteLine("ReviewsController.CreateReview: one or more reviews already created by current user");

        var review = _mapper.Map<Review>(reviewCreateDto);
        review.UserId = sessionInfo.UserId;

        _recipesDbContext.Reviews.Add(review);
        bool succesful = await _recipesDbContext.SaveChangesAsync() > 0;
        if (!succesful) return StatusCode(500, "Database operation failed");

        return CreatedAtAction(nameof(GetReviewById), new {review.Id}, _mapper.Map<ReviewResponseDto>(review));
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<ReviewResponseDto>> GetReviewById(Guid id)
    {
        var review = await _recipesDbContext.Reviews
            .FirstOrDefaultAsync(x => x.Id == id);

        if (review == null) return NotFound();
        
        return _mapper.Map<ReviewResponseDto>(review);
    }

    [HttpGet]
    [Route("get-all-by-recipe/{recipeId}")]
    public async Task<ActionResult<List<ReviewResponseDto>>> GetReviewsByRecipeId(Guid recipeId)
    {
        var reviews = await _recipesDbContext.Reviews
            .Where(r => r.RecipeId == recipeId)
            .ToListAsync();

        if (reviews == null) return NotFound();

        return _mapper.Map<List<ReviewResponseDto>>(reviews);   
    }

}
