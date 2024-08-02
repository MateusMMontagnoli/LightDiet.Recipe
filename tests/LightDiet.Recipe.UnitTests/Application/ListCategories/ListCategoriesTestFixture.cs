using LightDiet.Recipe.Domain.Entity;
using LightDiet.Recipe.Domain.Repository.Interfaces;
using LightDiet.Recipe.UnitTests.Common;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightDiet.Recipe.UnitTests.Application.ListCategories;

public class ListCategoriesTestFixture
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

    public List<Category> GetListOfCategories(int quantityOfItens)
    {
        var list = new List<Category>();

        for (int i = 0; i < quantityOfItens; i++)
        {
            list.Add(GetValidCategory());
        }

        return list;
    }

    public Mock<ICategoryRepository> GetRepositoryMock()
       => new();

}

[CollectionDefinition(nameof(ListCategoriesTestFixture))]
public class ListCategoriesTestFixtureCollecion
    : ICollectionFixture<ListCategoriesTestFixture>
{

}
