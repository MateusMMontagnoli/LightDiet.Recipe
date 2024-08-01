using LightDiet.Recipe.Application.Interfaces;
using LightDiet.Recipe.Application.UseCases.Category.Common.Dto;
using LightDiet.Recipe.Application.UseCases.Category.UpdateCategory.Dto;
using LightDiet.Recipe.Domain.Repository.Interfaces;
using MediatR;

namespace LightDiet.Recipe.Application.UseCases.Category.UpdateCategory;

public class UpdateCategory : IRequestHandler<UpdateCategoryInput, CategoryModelOutput>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategory(
        ICategoryRepository categoryRepository, 
        IUnitOfWork unitOfWork
        )
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryModelOutput> Handle(UpdateCategoryInput request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.Get(request.Id, cancellationToken);

        category.Update(request.Name, request.Description);

        if (request.IsActive != null)
        {
            if (request.IsActive != category.IsActive)
            {
                if ((bool)request.IsActive!)
                {
                    category.Activate();
                }
                else
                {
                    category.Deactivate();
                }
            }
        }

        await _categoryRepository.Update(category, cancellationToken);
        
        await _unitOfWork.Commit(cancellationToken);

        return CategoryModelOutput.FromCategory(category);
    }
}
