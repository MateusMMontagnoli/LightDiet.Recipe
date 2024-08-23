using Entities = LightDiet.Recipe.Domain.Entity;
using LightDiet.Recipe.EndToEndTests.Common;

namespace LightDiet.Recipe.EndToEndTests.Api.Category.Common;

public class CategoryApiBaseFixture 
    : BaseFixture
{
    public CategoryPersistence Persistence;

    public CategoryApiBaseFixture()
        : base() => Persistence = new CategoryPersistence(
            CreateDbContext()
        );

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

    public string GetInvalidNameTooShort()
    {
        var invalidInputByNameMinLength = GetValidCategoryName();

        invalidInputByNameMinLength = invalidInputByNameMinLength.Substring(0, 2);

        return invalidInputByNameMinLength;
    }

    public string GetInvalidNameTooLong()
    {
        var invalidName = string.Empty;

        while (invalidName.Length <= 50)
        {
            var name = Faker.Commerce.ProductName();

            invalidName += $" {name}";
        }

        return invalidName;
    }

    public string? GetInvalidNameByNullName()
    {
        return null;
    }

    public string GetInvalidNameByEmptyName()
    {
        return string.Empty;
    }

    public string? GetInvalidDescriptionByNullDescription()
    {
        return null;
    }

    public string GetInvalidDescriptionTooLong()
    {
        var invalidDescription = string.Empty;

        while (invalidDescription.Length <= 150)
        {
            var description = Faker.Commerce.ProductDescription();

            invalidDescription += $" {description}";
        }

        return invalidDescription;
    }

    public Entities.Category GetExampleCategory()
        => new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());

    public List<Entities.Category> GetExampleCategories(int numberOfCategoriesToCreate)
        => Enumerable.Range(1, numberOfCategoriesToCreate).Select(_ => 
            GetExampleCategory()
        ).ToList();
}
