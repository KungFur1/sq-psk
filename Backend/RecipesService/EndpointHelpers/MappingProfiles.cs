using AutoMapper;
using Contracts;
using RecipesService.DTOs;
using RecipesService.Models;

namespace RecipesService.EndpointHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Recipe, RecipeResponseDto>();
        CreateMap<RecipeCreateDto, Recipe>();
        // RecipeUpdateDto for now will have to be done manually

        CreateMap<RecipeResponseDto, RecipeCreated>();
        CreateMap<Recipe, RecipeUpdated>();
    }
}
