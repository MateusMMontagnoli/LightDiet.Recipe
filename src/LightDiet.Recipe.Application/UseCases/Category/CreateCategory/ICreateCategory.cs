using LightDiet.Recipe.Application.UseCases.Category.Common.Dto;
using LightDiet.Recipe.Application.UseCases.Category.CreateCategory.Dto;
using MediatR;

namespace LightDiet.Recipe.Application.UseCases.Category.CreateCategory;
public interface ICreateCategory : 
    IRequestHandler<CreateCategoryInput, CategoryModelOutput>
{

}
