using LightDiet.Recipe.Application.Common;
using LightDiet.Recipe.Application.UseCases.Category.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightDiet.Recipe.Application.UseCases.Category.ListCategories.Dto;

public class ListCategoriesOutput
    : PaginatedListOutput<CategoryModelOutput>
{
    public ListCategoriesOutput(
        int page, 
        int perPage, 
        int total, 
        IReadOnlyList<CategoryModelOutput> items) 
        : base(page, perPage, total, items)
    {

    }
}
