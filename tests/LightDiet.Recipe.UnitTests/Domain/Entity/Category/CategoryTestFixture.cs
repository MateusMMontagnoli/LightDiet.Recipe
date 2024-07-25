using LightDiet.Recipe.UnitTests.Common;
using DomainEntity = LightDiet.Recipe.Domain.Entity;

namespace LightDiet.Recipe.UnitTests.Domain.Entity.Category;

public class CategoryTestFixture : BaseFixture
{
    public CategoryTestFixture()
        : base() { }

    public string GetValidCategoryName()
    {
        var categoryName = "";

        while(categoryName.Length < 3)
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

    public DomainEntity.Category GetValidCategory()
        => new(
            GetValidCategoryName(),
            GetValidCategoryDescription()
        );
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection 
    : ICollectionFixture<CategoryTestFixture>
{ }