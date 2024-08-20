using LightDiet.Recipe.Application.Interfaces;
using LightDiet.Recipe.Application.UseCases.Category.CreateCategory;
using LightDiet.Recipe.Domain.Repository.Interfaces;
using LightDiet.Recipe.Infra.Data.EF.Repositories;
using LightDiet.Recipe.Infra.Data.EF.UnitOfWork;

namespace LightDiet.Recipe.Api.Configuration;

public static class UseCasesConfiguration
{
    public static IServiceCollection AddUseCases(
        this IServiceCollection services
    )
    {
        services.AddMediatR(config => 
            config.RegisterServicesFromAssembly(typeof(CreateCategory).Assembly)
        );
        services.AddRepositories();

        return services;
    }

    private static IServiceCollection AddRepositories(
       this IServiceCollection services
    )
    {
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
