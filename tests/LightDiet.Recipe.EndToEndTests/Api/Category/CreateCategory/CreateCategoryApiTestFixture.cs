using LightDiet.Recipe.Application.UseCases.Category.CreateCategory.Dto;
using LightDiet.Recipe.EndToEndTests.Api.Category.Common;

namespace LightDiet.Recipe.EndToEndTests.Api.Category.CreateCategory;

public class CreateCategoryApiTestFixture
    : CategoryApiBaseFixture
{
    public CreateCategoryInput GetValidInput()
        => new(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
        );
}

[CollectionDefinition(nameof(CreateCategoryApiTestFixture))]
public class CreateCategoryApiTestFixtureCollection
    : ICollectionFixture<CreateCategoryApiTestFixture>
{
    
}
