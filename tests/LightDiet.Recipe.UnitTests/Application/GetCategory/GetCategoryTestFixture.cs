using LightDiet.Recipe.Domain.Entity;
using LightDiet.Recipe.Domain.Repository.Interfaces;
using LightDiet.Recipe.UnitTests.Common;
using Moq;
using Xunit;

namespace LightDiet.Recipe.UnitTests.Application.GetCategory;

public class GetCategoryTestFixture : BaseFixture
{
    public GetCategoryTestFixture()
    : base() { }

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

    public Category GetValidCategory()
        => new Category(GetValidCategoryName(), GetValidCategoryDescription());

    public Mock<ICategoryRepository> GetRepositoryMock()
      => new();
}

[CollectionDefinition(nameof(GetCategoryTestFixture))]
public class GetCategoryTestFixtureCollection 
    : ICollectionFixture<GetCategoryTestFixture>
{
  
}
