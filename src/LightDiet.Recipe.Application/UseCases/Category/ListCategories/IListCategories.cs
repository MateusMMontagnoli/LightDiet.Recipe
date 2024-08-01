using LightDiet.Recipe.Application.UseCases.Category.ListCategories.Dto;
using MediatR;

namespace LightDiet.Recipe.Application.UseCases.Category.ListCategories;

public interface IListCategories
    : IRequestHandler<ListCategoriesInput, ListCategoriesOutput>
{
}
