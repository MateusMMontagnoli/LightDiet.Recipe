using Bogus;
using LightDiet.Recipe.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace LightDiet.Recipe.EndToEndTests.Common;

public class BaseFixture
{
    protected Faker Faker { get; set; }

    public CustomWebApplicationFactory<Program> WebAppFactory { get; set; }

    public HttpClient HttpClient { get; set; }

    public ApiClient ApiClient { get; set; }

    public BaseFixture()
    {
        Faker = new Faker("pt_BR");
        WebAppFactory = new CustomWebApplicationFactory<Program>();
        HttpClient = WebAppFactory.CreateClient();
        ApiClient = new ApiClient(HttpClient);
    }

    public LightDietRecipeDbContext CreateDbContext()
    {
        var dbContext = new LightDietRecipeDbContext(
            new DbContextOptionsBuilder<LightDietRecipeDbContext>()
            .UseInMemoryDatabase("end2end-tests-db")
            .Options
        );

        return dbContext;
    }

    
}
