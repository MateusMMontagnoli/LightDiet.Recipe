using LightDiet.Recipe.Domain.Entity;
using LightDiet.Recipe.Infra.Data.EF;
using LightDiet.Recipe.IntegrationTests.Common;
using Microsoft.EntityFrameworkCore;

namespace LightDiet.Recipe.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;

public class CategoryRepositoryTestFixture
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

    public LightDietRecipeDbContext CreateDbContext()
    {
        var dbContext = new LightDietRecipeDbContext(
            new DbContextOptionsBuilder<LightDietRecipeDbContext>()
            .UseInMemoryDatabase("integration-tests-db")
            .Options
        );

        return dbContext;
    }
}

[CollectionDefinition(nameof(CategoryRepositoryTestFixture))]
public class CategoryRepositoryTestFixtureCollection
    : ICollectionFixture<CategoryRepositoryTestFixture>
{

}
