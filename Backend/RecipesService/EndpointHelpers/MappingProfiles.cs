using AutoMapper;
using Contracts;
using RecipesService.DTOs;
using RecipesService.Models;

namespace RecipesService.EndpointHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Recipe, RecipeResponseDto>()
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Reviews.Any() ? (float)src.Reviews.Average(r => r.StarRating) : 0));
        CreateMap<RecipeCreateDto, Recipe>();
        // RecipeUpdateDto for now will have to be done manually

        CreateMap<RecipeResponseDto, RecipeCreated>();
        CreateMap<Recipe, RecipeUpdated>();

        CreateMap<ReviewCreateDto, Review>();
        CreateMap<Review, ReviewResponseDto>();
    }
}
