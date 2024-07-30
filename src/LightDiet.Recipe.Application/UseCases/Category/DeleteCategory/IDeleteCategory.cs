using LightDiet.Recipe.Application.UseCases.Category.DeleteCategory.Dto;
using MediatR;

namespace LightDiet.Recipe.Application.UseCases.Category.DeleteCategory;

public interface IDeleteCategory
    : IRequestHandler<DeleteCategoryInput, Unit>
{

}
