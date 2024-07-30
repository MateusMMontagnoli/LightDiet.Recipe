using LightDiet.Recipe.Application.Interfaces;
using LightDiet.Recipe.Domain.Entity;
using LightDiet.Recipe.Domain.Repository.Interfaces;
using LightDiet.Recipe.UnitTests.Common;
using Moq;

namespace LightDiet.Recipe.UnitTests.Application.DeleteCategory;

public class DeleteCategoryTestFixture : BaseFixture
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

    public Category GetValidCategory()
        => new(
            GetValidCategoryName(),
            GetValidCategoryDescription()
        );

    public Mock<ICategoryRepository> GetRepositoryMock()
       => new();

    public Mock<IUnitOfWork> GetUnitOfWorkMock()
        => new();
}

[CollectionDefinition(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTestFixtureCollection 
    : ICollectionFixture<DeleteCategoryTestFixture>
{

}