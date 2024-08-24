using LightDiet.Recipe.EndToEndTests.Api.Category.Common;

namespace LightDiet.Recipe.EndToEndTests.Api.Category.DeleteCategory;

public class DeleteCategoryApiTestFixture
    : CategoryApiBaseFixture
{ }

[CollectionDefinition(nameof(DeleteCategoryApiTestFixture))]
public class DeleteCategoryApiTestFixtureCollection
    : ICollectionFixture<DeleteCategoryApiTestFixture>
{ }
