using LightDiet.Recipe.IntegrationTests.Application.UseCases.Category.Common;
using LightDiet.Recipe.IntegrationTests.Common;

namespace LightDiet.Recipe.IntegrationTests.Application.UseCases.Category.GetCategory;

public class GetCategoryTestFixture
    : CategoryUseCasesBaseFixture
{

}


[CollectionDefinition(nameof(GetCategoryTestFixture))]
public class GetCategoryTestFixtureCollection
    : ICollectionFixture<GetCategoryTestFixture>
{

}