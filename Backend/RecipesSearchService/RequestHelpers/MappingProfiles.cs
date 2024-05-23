using AutoMapper;
using Contracts;
using RecipesSearchService.Models;

namespace RecipesSearchService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<RecipeCreated, RecipeSearchItem>();
    }
}
