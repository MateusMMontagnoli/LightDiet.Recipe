using Bogus;
using LightDiet.Recipe.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace LightDiet.Recipe.EndToEndTests.Common;

public class BaseFixture
{
    protected Faker Faker { get; set; }

    public BaseFixture()
        => Faker = new Faker("pt_BR");

    public LightDietRecipeDbContext CreateDbContext()
    {
        var dbContext = new LightDietRecipeDbContext(
            new DbContextOptionsBuilder<LightDietRecipeDbContext>()
            .UseInMemoryDatabase("end2end-tests-db")
            .Options
        );

        return dbContext;
    }

    public ApiClient ApiClient { get; set; }
}
