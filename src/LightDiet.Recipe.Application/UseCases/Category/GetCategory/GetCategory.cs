using LightDiet.Recipe.Application.Interfaces;
using LightDiet.Recipe.Application.UseCases.Category.Common.Dto;
using LightDiet.Recipe.Application.UseCases.Category.GetCategory.Dto;
using LightDiet.Recipe.Domain.Repository.Interfaces;
using LightDiet.Recipe.Domain.SeedWork;
using MediatR;

namespace LightDiet.Recipe.Application.UseCases.Category.GetCategory;

public class GetCategory : IGetCategory
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategory(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryModelOutput> Handle(
        GetCategoryInput request,
        CancellationToken cancellationToken
    )
    {
        var category = await _categoryRepository.Get(
            request.Id, 
            cancellationToken
        );

        return CategoryModelOutput.FromCategory(category);
    }
}
