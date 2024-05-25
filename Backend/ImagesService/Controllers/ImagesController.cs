using ImagesService.DTOs;
using ImagesService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImagesService;

[ApiController]
[Route("api/images")]
public class ImagesController : ControllerBase
{
    private readonly ImagesDbContext _imagesDbContext;

    public ImagesController(ImagesDbContext imagesDbContext)
    {
        _imagesDbContext = imagesDbContext;
    }
    [HttpPost]
    public async Task<ActionResult<ImageUploadResponseDto>> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("No image uploaded");
        System.Console.WriteLine(Path.GetExtension(file.FileName).ToString());
        if (Path.GetExtension(file.FileName).ToString() != ".jpg") return BadRequest("Unsupported file type");


        var imageMetadata = new ImageMetaData
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
        };
        imageMetadata.FilePath = Path.Combine("Images", imageMetadata.Id.ToString() + Path.GetExtension(file.FileName));


        var directory = Path.GetDirectoryName(imageMetadata.FilePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        using (var stream = new FileStream(imageMetadata.FilePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        _imagesDbContext.ImagesMetaData.Add(imageMetadata);
        await _imagesDbContext.SaveChangesAsync();

        return Ok(new ImageUploadResponseDto {ImageId = imageMetadata.Id});
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult> RetrieveImageById(Guid id)
    {
        var imageMetaData = await _imagesDbContext.ImagesMetaData
            .FirstOrDefaultAsync(x => x.Id == id);
        
        if (imageMetaData == null) return NotFound();
        if (!System.IO.File.Exists(imageMetaData.FilePath)) return StatusCode(500, "Server error: missing image file");

        var memoryStream = new MemoryStream();
        using (var stream = new FileStream(imageMetaData.FilePath, FileMode.Open))
        {
            await stream.CopyToAsync(memoryStream);
        }
        memoryStream.Position = 0;

        return File(memoryStream, "image/jpeg", Path.GetFileName(imageMetaData.FilePath));
    }
}
