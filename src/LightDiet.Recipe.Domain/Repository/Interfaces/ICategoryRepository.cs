using LightDiet.Recipe.Domain.Entity;
using LightDiet.Recipe.Domain.SeedWork;
using LightDiet.Recipe.Domain.SeedWork.SearchableRepository;

namespace LightDiet.Recipe.Domain.Repository.Interfaces;

public interface ICategoryRepository 
    : IGenericRepository<Category>,
    ISearchableRepository<Category>
{

}
