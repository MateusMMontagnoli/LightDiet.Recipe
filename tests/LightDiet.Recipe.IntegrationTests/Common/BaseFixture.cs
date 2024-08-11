using Bogus;
using LightDiet.Recipe.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace LightDiet.Recipe.IntegrationTests.Common;

public class BaseFixture
{
    protected Faker Faker { get; set; }

    public BaseFixture()
        => Faker = new Faker("pt_BR");

    public LightDietRecipeDbContext CreateDbContext(bool preserveData = false)
    {
        var dbContext = new LightDietRecipeDbContext(
            new DbContextOptionsBuilder<LightDietRecipeDbContext>()
            .UseInMemoryDatabase("integration-tests-db")
            .Options
        );

        if (!preserveData)
        {
            dbContext.Database.EnsureDeleted();
        }

        return dbContext;
    }
}
