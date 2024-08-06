using LightDiet.Recipe.Domain.Entity;
using LightDiet.Recipe.Infra.Data.EF.Configuration;
using Microsoft.EntityFrameworkCore;
//using System.Reflection;

namespace LightDiet.Recipe.Infra.Data.EF;

public class LightDietRecipeDbContext
    : DbContext
{
    public DbSet<Category> Categories => Set<Category>();

    public LightDietRecipeDbContext(
        DbContextOptions<LightDietRecipeDbContext> options
    )
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());

        //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
