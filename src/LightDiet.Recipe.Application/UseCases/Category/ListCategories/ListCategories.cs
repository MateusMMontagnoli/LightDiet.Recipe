using LightDiet.Recipe.Application.UseCases.Category.Common.Dto;
using LightDiet.Recipe.Application.UseCases.Category.ListCategories.Dto;
using LightDiet.Recipe.Domain.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightDiet.Recipe.Application.UseCases.Category.ListCategories;

public class ListCategories 
    : IListCategories
{
    private readonly ICategoryRepository _categoryRepository;

    public ListCategories(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ListCategoriesOutput> Handle(ListCategoriesInput request, CancellationToken cancellationToken)
    {
        var searchOutput = await _categoryRepository.Search(
            new(
                request.Page, 
                request.PerPage, 
                request.Search, 
                request.Sort, 
                request.Dir
            ),
            cancellationToken
        );

        var output = new ListCategoriesOutput(
            searchOutput.CurrentPage,
            searchOutput.PerPage,
            searchOutput.Total,
            searchOutput.Items
                .Select(CategoryModelOutput.FromCategory)
                .ToList()
        );

        return output;
    }
}
