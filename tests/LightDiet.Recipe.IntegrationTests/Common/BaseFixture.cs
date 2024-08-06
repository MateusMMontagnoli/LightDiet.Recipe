using Bogus;

namespace LightDiet.Recipe.IntegrationTests.Common;

public class BaseFixture
{
    protected Faker Faker { get; set; }

    public BaseFixture()
        => Faker = new Faker("pt_BR");

}
