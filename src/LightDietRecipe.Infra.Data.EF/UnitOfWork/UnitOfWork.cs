using LightDiet.Recipe.Application.Interfaces;

namespace LightDiet.Recipe.Infra.Data.EF.UnitOfWork;

public class UnitOfWork
    : IUnitOfWork
{
    private readonly LightDietRecipeDbContext _context;

    public UnitOfWork(LightDietRecipeDbContext context)
    {
        _context = context;
    }

    public async Task Commit(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task Rollback(CancellationToken cancellationToken)
        => Task.CompletedTask;
}
