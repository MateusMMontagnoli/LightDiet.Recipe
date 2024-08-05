using LightDiet.Recipe.Application.Common;
using LightDiet.Recipe.Domain.SeedWork.SearchableRepository;
using MediatR;

namespace LightDiet.Recipe.Application.UseCases.Category.ListCategories.Dto;

public class ListCategoriesInput
    : PaginatedListInput, IRequest<ListCategoriesOutput>
{
    public ListCategoriesInput(
        int page = 1, 
        int perPage = 15, 
        string search = "", 
        string sort = "", 
        SearchOrder dir = SearchOrder.Asc) 
        : base(page, perPage, search, sort, dir)
    {

    }
}
