using LightDiet.Recipe.Application.Interfaces;
using LightDiet.Recipe.Application.UseCases.Category.CreateCategory.Dto;
using LightDiet.Recipe.Domain.Repository.Interfaces;
using LightDiet.Recipe.UnitTests.Common;
using Moq;

namespace LightDiet.Recipe.UnitTests.Application.CreateCategory;
public class CreateCategoryTesteFixture : BaseFixture
{
    public CreateCategoryTesteFixture()
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

    public bool GetRandomBoolean() 
        => (new Random()).NextDouble() < 0.5;
    
    public CreateCategoryInput GetValidInput()
        => new(
            GetValidCategoryName(),
            GetValidCategoryDescription(), 
            GetRandomBoolean()
        );

    public Mock<ICategoryRepository> GetRepositoryMock()
        => new();

    public Mock<IUnitOfWork> GetUnitOfWorkMock()
        => new();

}

[CollectionDefinition(nameof(CreateCategoryTesteFixture))]
public class CreateCategoryTesteFixtureCollection
    : ICollectionFixture<CreateCategoryTesteFixture>
{ }