using LightDiet.Recipe.Domain.Entity;
using LightDiet.Recipe.IntegrationTests.Common;

namespace LightDiet.Recipe.IntegrationTests.Infra.Data.EF.UnitOfWork;


public class UnitOfWorkTestFixture
    : BaseFixture
{
    public string GetValidCategoryName()
    {
        var categoryName = "";

        while (categoryName.Length < 3)
        {
            categoryName = Faker.Commerce.Categories(1)[0];
        }

        if (categoryName.Length > 50)
        {
            categoryName = categoryName[..50];
        }

        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = "";

        categoryDescription = Faker.Commerce.ProductDescription();

        if (categoryDescription.Length > 150)
        {
            categoryDescription = categoryDescription[..150];
        }

        return categoryDescription;
    }

    public bool GetRandomBoolean()
        => new Random().NextDouble() < 0.5;

    public Category GetValidCategory()
    => new(
           GetValidCategoryName(),
           GetValidCategoryDescription(),
           GetRandomBoolean()
       );

    public List<Category> GetValidCategoriesList(int length = 10)
    => Enumerable
        .Range(1, length)
        .Select(_ => GetValidCategory())
        .ToList();
}

[CollectionDefinition(nameof(UnitOfWorkTestFixture))]
public class UnitOfWorkTestFixtureCollection
    : ICollectionFixture<UnitOfWorkTestFixture>
{

}
