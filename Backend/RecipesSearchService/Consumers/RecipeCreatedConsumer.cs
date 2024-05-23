using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using RecipesSearchService.Models;

namespace RecipesSearchService.Consumers;

public class RecipeCreatedConsumer : IConsumer<RecipeCreated>
{
    private readonly IMapper _mapper;

    public RecipeCreatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<RecipeCreated> consumable)
    {
        Console.WriteLine("--> Consuming RecipeCreated id: " + consumable.Message.Id);

        var recipeSearchItem = _mapper.Map<RecipeSearchItem>(consumable.Message);

        await recipeSearchItem.SaveAsync();
    }
}
