using LightDiet.Recipe.Application.UseCases.Category.Common.Dto;
using LightDiet.Recipe.Application.UseCases.Category.GetCategory.Dto;
using MediatR;

namespace LightDiet.Recipe.Application.UseCases.Category.GetCategory;

public interface IGetCategory : IRequestHandler<GetCategoryInput, CategoryModelOutput>
{

}
