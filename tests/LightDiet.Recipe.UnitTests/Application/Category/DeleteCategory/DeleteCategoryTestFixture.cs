using LightDiet.Recipe.UnitTests.Application.Category.Common;

namespace LightDiet.Recipe.UnitTests.Application.Category.DeleteCategory;

public class DeleteCategoryTestFixture : CategoryUseCasesBaseFixture
{

}

[CollectionDefinition(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTestFixtureCollection
    : ICollectionFixture<DeleteCategoryTestFixture>
{

}