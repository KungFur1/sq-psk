using ImagesService.Models;
using Microsoft.EntityFrameworkCore;

namespace ImagesService;

public class ImagesDbContext : DbContext
{
    public ImagesDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<ImageMetaData> ImagesMetaData { get; set; }
}
