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
        var total = await _categories.CountAsync();

        var category = await _categories.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id,
            cancellationToken
        );

        NotFoundException.ThrowIfNull(category, $"Category '{id}' not found");

        return category!;
    }

    public Task Delete(Category aggregate, CancellationToken cancellationToken)
        => Task.FromResult(_categories.Remove(aggregate));

    public async Task<SearchOutput<Category>> Search(SearchInput input, CancellationToken cancellationToken)
    {

        var toSkip = (input.Page - 1) * input.PerPage;
        var query = _categories.AsNoTracking();
        query = AddOrderToQuery(query, input.OrderBy, input.Order);

        if (!string.IsNullOrWhiteSpace(input.Search))
        {
            query = query.Where(x =>
                x.Name.Contains(input.Search)
            );
        }

        var total = await query.CountAsync(cancellationToken);

        if (total == 0)
        {
            return new(input.Page, input.PerPage, total, []);
        }

        var items = await query
            .Skip(toSkip)
            .Take(input.PerPage)
            .ToListAsync(cancellationToken);

        return new(input.Page, input.PerPage, total, items);
    }

    private IQueryable<Category> AddOrderToQuery(
        IQueryable<Category> query,
        string propToOrderBy,
        SearchOrder order
    )
    {
        return (propToOrderBy.ToLower(), order) switch
        {
            ("name", SearchOrder.Asc) => query.OrderBy(x => x.Name),
            ("name", SearchOrder.Desc) => query.OrderByDescending(x => x.Name),
            _ => query.OrderBy(x => x.Name)
        };
    }

    public Task Update(Category aggregate, CancellationToken cancellationToken)
        => Task.FromResult(_categories.Update(aggregate));
    
}
