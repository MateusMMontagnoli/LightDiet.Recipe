using LightDiet.Recipe.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace LightDiet.Recipe.Api.Configuration;

public static class ConnectionConfiguration
{
    public static IServiceCollection AddProjectConnections(this IServiceCollection services)
    {
        services.AddDbConnection();

        return services;
    }

    private static IServiceCollection AddDbConnection(this IServiceCollection services)
    {
        services.AddDbContext<LightDietRecipeDbContext>(
            options => options.UseInMemoryDatabase(
                "InMemory-DSV-Database"
            )
        );

        return services;
    }

}
