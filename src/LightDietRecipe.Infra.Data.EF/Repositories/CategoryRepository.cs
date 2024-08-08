using LightDiet.Recipe.Application.Exceptions;
using LightDiet.Recipe.Domain.Entity;
using LightDiet.Recipe.Domain.Repository.Interfaces;
using LightDiet.Recipe.Domain.SeedWork.SearchableRepository;
using Microsoft.EntityFrameworkCore;

namespace LightDiet.Recipe.Infra.Data.EF.Repositories;

public class CategoryRepository
    : ICategoryRepository
{
    private readonly LightDietRecipeDbContext _context;
    private DbSet<Category> _categories =>
        _context.Set<Category>();

    public CategoryRepository(LightDietRecipeDbContext context)
    {
        _context = context;
    }

    public async Task Insert(
        Category aggregate,
        CancellationToken cancellationToken
    )
        => await _categories.AddAsync(aggregate, cancellationToken);

    public async Task<Category> Get(Guid id, CancellationToken cancellationToken)
    {
        var category = await _categories.FindAsync(
            new object[] { id },
            cancellationToken
        );

        NotFoundException.ThrowIfNull(category, $"Category '{id}' not found");

        return category!;
    }

    public Task Delete(Category aggregate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    

    public Task<SearchOutput<Category>> Search(SearchInput input, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Update(Category aggregate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
