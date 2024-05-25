using AutoMapper;
using Contracts;
using MassTransit;
using ImagesService.Models;
using Microsoft.EntityFrameworkCore;
using ImagesService.EndpointHelpers;

namespace ImagesService.Consumers;

public class RecipeUpdatedConsumer : IConsumer<RecipeUpdated>
{
    private readonly ImagesDbContext _imagesDbContext;

    public RecipeUpdatedConsumer(ImagesDbContext imagesDbContext)
    {
        _imagesDbContext = imagesDbContext;
    }
    public async Task Consume(ConsumeContext<RecipeUpdated> consumable)
    {
        Console.WriteLine("--> Consuming RecipeUpdated id: " + consumable.Message.Id);

        var imageMetaData = await _imagesDbContext.ImagesMetaData
            .FirstOrDefaultAsync(x => x.RecipeId == consumable.Message.Id);
        
        if (imageMetaData == null)
        {
            Console.WriteLine("While consuming: a recipe with no matching image detected");
            return;
        }

        // Delete image existing image if the recipe was assigned a new image
        if (imageMetaData.Id != consumable.Message.ImageId)
        {
            if (File.Exists(imageMetaData.FilePath)) File.Delete(imageMetaData.FilePath);
            _imagesDbContext.ImagesMetaData.Remove(imageMetaData);
            var succesful = await _imagesDbContext.SaveChangesAsync() > 0;
            if (!succesful) Console.WriteLine("While consuming: failed to update database");
        }

    }
}
