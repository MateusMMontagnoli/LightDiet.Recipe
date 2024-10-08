﻿using LightDiet.Recipe.Application.UseCases.Category.UpdateCategory.Dto;
using LightDiet.Recipe.IntegrationTests.Application.UseCases.Category.Common;

namespace LightDiet.Recipe.IntegrationTests.Application.UseCases.Category.UpdateCategory;


public class UpdateCategoryTestFixture
    : CategoryUseCasesBaseFixture
{
    public UpdateCategoryInput GetValidInput(Guid? id = null)
       => new(
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
}

[CollectionDefinition(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTestFixtureCollection
    : ICollectionFixture<UpdateCategoryTestFixture>
{

}
