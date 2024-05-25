namespace ImagesService.Models;

public class ImageMetaData
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid? RecipeId { get; set; }
    public string FilePath { get; set; }
}
