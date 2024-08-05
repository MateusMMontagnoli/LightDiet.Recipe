using LightDiet.Recipe.UnitTests.Application.Category.Common;

namespace LightDiet.Recipe.UnitTests.Application.Category.GetCategory;

public class GetCategoryTestFixture : CategoryUseCasesBaseFixture
{
    public GetCategoryTestFixture()
    : base() { }

}

[CollectionDefinition(nameof(GetCategoryTestFixture))]
public class GetCategoryTestFixtureCollection
    : ICollectionFixture<GetCategoryTestFixture>
{

}
