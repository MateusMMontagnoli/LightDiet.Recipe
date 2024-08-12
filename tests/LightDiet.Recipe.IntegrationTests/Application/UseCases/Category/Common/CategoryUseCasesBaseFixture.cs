using LightDiet.Recipe.IntegrationTests.Common;
using DomainEntities = LightDiet.Recipe.Domain.Entity;

namespace LightDiet.Recipe.IntegrationTests.Application.UseCases.Category.Common;

public class CategoryUseCasesBaseFixture : BaseFixture
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

    public DomainEntities.Category GetValidCategory()
    => new(
           GetValidCategoryName(),
           GetValidCategoryDescription(),
           GetRandomBoolean()
       );

    public List<DomainEntities.Category> GetValidCategoriesList(int length = 10)
    => Enumerable
        .Range(1, length)
        .Select(_ => GetValidCategory())
        .ToList();
}
