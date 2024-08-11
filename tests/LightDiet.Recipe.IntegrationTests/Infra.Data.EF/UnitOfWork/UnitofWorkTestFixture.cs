using LightDiet.Recipe.IntegrationTests.Common;

namespace LightDiet.Recipe.IntegrationTests.Infra.Data.EF.UnitOfWork;

[CollectionDefinition(nameof(UnitOfWorkTestFixture))]
public class UnitofWorkTestFixture
    : BaseFixture
{
}

public class UnitOfWorkTestFixtureCollection
    : ICollectionFixture<UnitofWorkTestFixture>
{

}
