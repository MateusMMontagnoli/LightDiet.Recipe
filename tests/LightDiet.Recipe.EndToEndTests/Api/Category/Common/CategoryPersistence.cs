using LightDiet.Recipe.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
using Entities = LightDiet.Recipe.Domain.Entity;

namespace LightDiet.Recipe.EndToEndTests.Api.Category.Common;

public class CategoryPersistence(LightDietRecipeDbContext context)
{
    private readonly LightDietRecipeDbContext _context = context;

    public async Task<Entities.Category?> GetById(Guid id)
        => await _context
        .Categories
        .AsNoTracking()
        .FirstOrDefaultAsync(x => 
            x.Id == id
        );

    public async Task InsertList(List<Entities.Category> categories)
    {
        await _context.Categories.AddRangeAsync(categories);

        await _context.SaveChangesAsync();
    }
}
