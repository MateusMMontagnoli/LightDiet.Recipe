using LightDiet.Recipe.IntegrationTests.Application.UseCases.Category.Common;

namespace LightDiet.Recipe.IntegrationTests.Application.UseCases.Category.DeleteCategory;

public class DeleteCategoryTestFixture
    : CategoryUseCasesBaseFixture
{

}

[CollectionDefinition(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTestFixtureCollection
    : ICollectionFixture<DeleteCategoryTestFixture>
{

}
