using LightDiet.Recipe.Domain.SeedWork.SearchableRepository;
using LightDiet.Recipe.IntegrationTests.Application.UseCases.Category.Common;
using Entities = LightDiet.Recipe.Domain.Entity;

namespace LightDiet.Recipe.IntegrationTests.Application.UseCases.Category.ListCategories;

public class ListCategoriesTestFixture
    : CategoryUseCasesBaseFixture
{
    public List<Entities.Category> GetValidCategoriesListWithNames(List<string> names)
        => names.Select(name =>
        {
            var category = GetValidCategory();

            category.Update(name);

            return category;
        }).ToList();

    public List<Entities.Category> OrderCategoryList(List<Entities.Category> categoriesOriginalList, string orderBy, SearchOrder order)
    {
        var categoriesOrderedList = new List<Entities.Category>(categoriesOriginalList);

        categoriesOrderedList = (orderBy.ToLower(), order) switch
        {
            ("name", SearchOrder.Desc) => [.. categoriesOrderedList.OrderByDescending(x => x.Name)],
            ("name", SearchOrder.Asc) => [.. categoriesOrderedList.OrderBy(x => x.Name)],
            ("id", SearchOrder.Asc) => [.. categoriesOrderedList.OrderBy(x => x.Id)],
            ("id", SearchOrder.Desc) => [.. categoriesOrderedList.OrderByDescending(x => x.Id)],
            ("createdat", SearchOrder.Asc) => [.. categoriesOrderedList.OrderBy(x => x.CreatedAt)],
            ("createdat", SearchOrder.Desc) => [.. categoriesOrderedList.OrderByDescending(x => x.CreatedAt)],
            _ => [.. categoriesOrderedList.OrderBy(x => x.Name)]
        };

        return [.. categoriesOrderedList];
    }
}

[CollectionDefinition(nameof(ListCategoriesTestFixture))]
public class ListCategoriesTestFixtureCollection
    : ICollectionFixture<ListCategoriesTestFixture>
{

}
