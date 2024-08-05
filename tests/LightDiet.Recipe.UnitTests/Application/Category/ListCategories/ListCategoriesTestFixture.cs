using LightDiet.Recipe.Application.UseCases.Category.ListCategories.Dto;
using LightDiet.Recipe.Domain.SeedWork.SearchableRepository;
using LightDiet.Recipe.UnitTests.Application.Category.Common;
using Entities = LightDiet.Recipe.Domain.Entity;

namespace LightDiet.Recipe.UnitTests.Application.Category.ListCategories;

public class ListCategoriesTestFixture
    : CategoryUseCasesBaseFixture
{
    public List<Entities.Category> GetListOfCategories(int quantityOfItens)
    {
        var list = new List<Entities.Category>();

        for (int i = 0; i < quantityOfItens; i++)
        {
            list.Add(GetValidCategory());
        }

        return list;
    }

    public ListCategoriesInput GetExampleInput()
    {
        var random = new Random();

        return new ListCategoriesInput(
            page: random.Next(1, 10),
            perPage: random.Next(15, 100),
            search: Faker.Commerce.ProductName(),
            sort: Faker.Commerce.ProductName(),
            dir: random.Next(0, 10) > 5 ? SearchOrder.Asc : SearchOrder.Desc
        );
    }
}

[CollectionDefinition(nameof(ListCategoriesTestFixture))]
public class ListCategoriesTestFixtureCollecion
    : ICollectionFixture<ListCategoriesTestFixture>
{

}
