using LightDiet.Recipe.Application.UseCases.Category.CreateCategory.Dto;

namespace LightDiet.Recipe.Application.UseCases.Category.CreateCategory;
public interface ICreateCategory
{
    public Task<CreateCategoryOutput> Handle(CreateCategoryInput input, CancellationToken cancellationToken);
}
