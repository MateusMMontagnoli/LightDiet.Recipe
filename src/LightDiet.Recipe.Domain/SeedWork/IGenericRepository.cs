namespace LightDiet.Recipe.Domain.SeedWork;

public interface IGenericRepository<Taggregate> : IRepository
{
    public Task Insert(Taggregate aggregate, CancellationToken cancellationToken);

    public Task<Taggregate> Get(Guid id, CancellationToken cancellationToken);
}
