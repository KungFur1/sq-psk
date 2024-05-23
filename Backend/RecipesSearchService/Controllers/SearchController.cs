using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using RecipesSearchService.Models;
using RecipesSearchService.RequestHelpers;

namespace RecipesSearchService;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<RecipeSearchItem>>> SearchRecipeItems([FromQuery]SearchParams searchParams)
    {
        if (searchParams.PageNumber < 1 || searchParams.PageSize < 1 || searchParams.PageSize > 100) return BadRequest("PageNumber or PageSize value out of range"); 


        var query = DB.PagedSearch<RecipeSearchItem, RecipeSearchItem>();

        query.Sort(x => x.Ascending(a => a.CreatedAt));
        if (!string.IsNullOrEmpty(searchParams.SearchTerm))
        {
            query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
        }

        query = searchParams.OrderBy switch
        {
            "title" => query.Sort(x => x.Ascending(a => a.Title)),
            "new" => query.Sort(x => x.Descending(a => a.CreatedAt)),
            _ => query
        };

        query = searchParams.FilterBy switch
        {
            "new" => query.Match(x => x.CreatedAt > DateTime.UtcNow.AddMinutes(-2)),
            _ => query
        };

        if (!string.IsNullOrEmpty(searchParams.Title))
        {
            query.Match(x => x.Title.ToLower().Contains(searchParams.Title.ToLower()));
        }
        if (!string.IsNullOrEmpty(searchParams.ShortDescription))
        {
            query.Match(x => x.ShortDescription.ToLower().Contains(searchParams.ShortDescription.ToLower()));
        }
        if (!string.IsNullOrEmpty(searchParams.IngredientsList))
        {
            query.Match(x => x.IngredientsList.ToLower().Contains(searchParams.IngredientsList.ToLower()));
        }
        if (!string.IsNullOrEmpty(searchParams.CookingSteps))
        {
            query.Match(x => x.CookingSteps.ToLower().Contains(searchParams.CookingSteps.ToLower()));
        }

        query.PageNumber(searchParams.PageNumber);
        query.PageSize(searchParams.PageSize);


        var result = await query.ExecuteAsync();

        return Ok(new 
        {
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });
    }
}
