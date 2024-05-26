using MassTransit;
using Microsoft.EntityFrameworkCore;
using RecipesService.Models;

namespace RecipesService.Data;

public class RecipesDbContext : DbContext
{
    public RecipesDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}
