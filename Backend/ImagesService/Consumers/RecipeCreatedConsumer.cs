using AutoMapper;
using Contracts;
using MassTransit;
using ImagesService.Models;
using Microsoft.EntityFrameworkCore;

namespace ImagesService.Consumers;

public class RecipeCreatedConsumer : IConsumer<RecipeCreated>
{
    private readonly ImagesDbContext _imagesDbContext;

    public RecipeCreatedConsumer(ImagesDbContext imagesDbContext)
    {
        _imagesDbContext = imagesDbContext;
    }
    public async Task Consume(ConsumeContext<RecipeCreated> consumable)
    {
        Console.WriteLine("--> Consuming RecipeCreated id: " + consumable.Message.Id);

        var imageMetaData = await _imagesDbContext.ImagesMetaData
            .FirstOrDefaultAsync(x => x.Id == consumable.Message.ImageId);
        
        if (imageMetaData == null)
        {
            Console.WriteLine("While consuming: a recipe with no matching image detected");
            return;
        }

        imageMetaData.RecipeId = consumable.Message.Id;
        bool succesful = await _imagesDbContext.SaveChangesAsync() > 0;
        if (!succesful) Console.WriteLine("While consuming: failed to update database");
    }
}
