﻿namespace LightDiet.Recipe.Domain.SeedWork;

public interface IGenericRepository<Taggregate> : IRepository where Taggregate : AggregateRoot
{
    public Task Insert(Taggregate aggregate, CancellationToken cancellationToken);

    public Task<Taggregate> Get(Guid id, CancellationToken cancellationToken);

    public Task Delete(Taggregate aggregate, CancellationToken cancellationToken);

    public Task Update(Taggregate aggregate, CancellationToken cancellationToken);
}
