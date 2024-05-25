using AutoMapper;
using Contracts;
using MassTransit;
using ImagesService.Models;
using Microsoft.EntityFrameworkCore;
using ImagesService.EndpointHelpers;

namespace ImagesService.Consumers;

public class RecipeDeletedConsumer : IConsumer<RecipeDeleted>
{
    private readonly ImagesDbContext _imagesDbContext;

    public RecipeDeletedConsumer(ImagesDbContext imagesDbContext)
    {
        _imagesDbContext = imagesDbContext;
    }
    public async Task Consume(ConsumeContext<RecipeDeleted> consumable)
    {
        Console.WriteLine("--> Consuming RecipeDeleted id: " + consumable.Message.Id);
        
        var imageMetaData = await _imagesDbContext.ImagesMetaData
            .FirstOrDefaultAsync(x => x.RecipeId == consumable.Message.Id);
        
        if (imageMetaData == null)
        {
            Console.WriteLine("While consuming: a recipe with no matching image detected");
            return;
        }

        if (File.Exists(imageMetaData.FilePath)) File.Delete(imageMetaData.FilePath);
        _imagesDbContext.ImagesMetaData.Remove(imageMetaData);
        var succesful = await _imagesDbContext.SaveChangesAsync() > 0;
        if (!succesful) Console.WriteLine("While consuming: failed to update database");
    }
}
