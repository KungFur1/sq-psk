using Microsoft.EntityFrameworkCore;
using RecipesService.Models;

namespace RecipesService.Data;

public class RecipesDbContext : DbContext
{
    public RecipesDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Recipe> Recipes { get; set; }
}
