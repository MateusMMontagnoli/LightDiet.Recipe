﻿using LightDiet.Recipe.Application.Interfaces;
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

    public Mock<ICategoryRepository> GetRepositoryMock()
       => new();

    public Mock<IUnitOfWork> GetUnitOfWorkMock()
        => new();
}

[CollectionDefinition(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTestFixtureCollection : ICollectionFixture<UpdateCategoryTestFixture>
{

}
