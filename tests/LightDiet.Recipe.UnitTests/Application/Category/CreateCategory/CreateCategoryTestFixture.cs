using LightDiet.Recipe.Application.Interfaces;
using LightDiet.Recipe.Application.UseCases.Category.CreateCategory.Dto;
using LightDiet.Recipe.Domain.Repository.Interfaces;
using LightDiet.Recipe.UnitTests.Application.Category.Common;
using LightDiet.Recipe.UnitTests.Common;
using Moq;

namespace LightDiet.Recipe.UnitTests.Application.Category.CreateCategory;
public class CreateCategoryTestFixture : CategoryUseCasesBaseFixture
{
    public CreateCategoryTestFixture()
        : base() { }

    public CreateCategoryInput GetValidInput()
        => new(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
        );

    public CreateCategoryInput GetInvalidInputByShortName()
    {
        var invalidInputByNameMinLength = GetValidInput();

        invalidInputByNameMinLength.Name = invalidInputByNameMinLength.Name.Substring(0, 2);

        return invalidInputByNameMinLength;
    }

    public CreateCategoryInput GetInvalidInputByGreaterName()
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

    public CreateCategoryInput GetInvalidInputByNullName()
    {
        var invalidInputByNameNullity = GetValidInput();

        invalidInputByNameNullity.Name = null!;

        return invalidInputByNameNullity;
    }

    public CreateCategoryInput GetInvalidInputByEmptyName()
    {
        var invalidInputByNameEmpty = GetValidInput();

        invalidInputByNameEmpty.Name = string.Empty;

        return invalidInputByNameEmpty;
    }

    public CreateCategoryInput GetInvalidInputByNullDescription()
    {
        var invalidInputByDescriptionNullity = GetValidInput();

        invalidInputByDescriptionNullity.Description = null!;

        return invalidInputByDescriptionNullity;
    }

    public CreateCategoryInput GetInvalidInputByGreaterDescription()
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
}

[CollectionDefinition(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTestFixtureCollection
    : ICollectionFixture<CreateCategoryTestFixture>
{ }