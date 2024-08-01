using LightDiet.Recipe.Application.Interfaces;
using LightDiet.Recipe.Application.UseCases.Category.CreateCategory.Dto;
using LightDiet.Recipe.Application.UseCases.Category.UpdateCategory.Dto;
using LightDiet.Recipe.Domain.Entity;
using LightDiet.Recipe.Domain.Repository.Interfaces;
using LightDiet.Recipe.UnitTests.Common;
using Moq;
using Xunit;

namespace LightDiet.Recipe.UnitTests.Application.UpdateCategory;

public class UpdateCategoryTestFixture
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
        => (new Random()).NextDouble() < 0.5;


    public Category GetValidCategory()
        => new(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
        );

    public UpdateCategoryInput GetValidInput(Guid? id = null)
        => new (
                id ?? Guid.NewGuid(),
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                GetRandomBoolean()
            );

    public UpdateCategoryInput GetInvalidInputByShortName()
    {
        var invalidInputByNameMinLength = GetValidInput();

        invalidInputByNameMinLength.Name = invalidInputByNameMinLength.Name.Substring(0, 2);

        return invalidInputByNameMinLength;
    }

    public UpdateCategoryInput GetInvalidInputByGreaterName()
    {
        var invalidInputByNameMaxLength = GetValidInput();

        var invalidName = string.Empty;

        while (invalidName.Length <= 50)
        {
            var name = Faker.Commerce.ProductName();

            invalidName += $" {name}";
        }

        invalidInputByNameMaxLength.Name = invalidName;

        return invalidInputByNameMaxLength;
    }

    public UpdateCategoryInput GetInvalidInputByNullName()
    {
        var invalidInputByNameNullity = GetValidInput();

        invalidInputByNameNullity.Name = null!;

        return invalidInputByNameNullity;
    }

    public UpdateCategoryInput GetInvalidInputByEmptyName()
    {
        var invalidInputByNameEmpty = GetValidInput();

        invalidInputByNameEmpty.Name = string.Empty;

        return invalidInputByNameEmpty;
    }
 

    public UpdateCategoryInput GetInvalidInputByGreaterDescription()
    {
        var invalidInputByDescriptionMaxLength = GetValidInput();

        var invalidDescription = string.Empty;

        while (invalidDescription.Length <= 150)
        {
            var description = Faker.Commerce.ProductDescription();

            invalidDescription += $" {description}";
        }

        invalidInputByDescriptionMaxLength.Description = invalidDescription;

        return invalidInputByDescriptionMaxLength;
    }

    public Mock<ICategoryRepository> GetRepositoryMock()
       => new();

    public Mock<IUnitOfWork> GetUnitOfWorkMock()
        => new();
}

[CollectionDefinition(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTestFixtureCollection : ICollectionFixture<UpdateCategoryTestFixture>
{

}
