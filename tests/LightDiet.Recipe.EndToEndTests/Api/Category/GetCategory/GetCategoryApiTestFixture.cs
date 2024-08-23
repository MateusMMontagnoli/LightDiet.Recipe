using LightDiet.Recipe.EndToEndTests.Api.Category.Common;

namespace LightDiet.Recipe.EndToEndTests.Api.Category.GetCategory;

public class GetCategoryApiTestFixture
    : CategoryApiBaseFixture
{

}

[CollectionDefinition(nameof(GetCategoryApiTestFixture))]
public class GetCategoryApiTestFixtureCollection
    : ICollectionFixture<GetCategoryApiTestFixture>
{

}


