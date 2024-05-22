namespace RecipesSearchService.RequestHelpers;

public class SearchParams
{
    public string SearchTerm {get; set;}
    public int PageNumber {get; set;} = 1;
    public int PageSize {get; set;} = 4;
    public string OrderBy {get; set;}
    public string FilterBy {get; set;}
    public string Title {get; set;}
    public string ShortDescription {get; set;}
    public string IngredientsList {get; set;}
    public string CookingSteps {get; set;}
}
