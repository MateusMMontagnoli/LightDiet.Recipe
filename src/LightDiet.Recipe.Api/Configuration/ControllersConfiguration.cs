using LightDiet.Recipe.Api.Filters;

namespace LightDiet.Recipe.Api.Configuration;

public static class ControllersConfiguration
{
    public static IServiceCollection AddAndConfigureControllers(this IServiceCollection services)
    {
        services.AddControllers(options => 
            options.Filters.Add(typeof(ApiGlobalExceptionsFilter))
        );
        services.AddDocumentation();
        
        return services;
    }
    
    private static IServiceCollection AddDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    public static WebApplication UseDocumentation(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        return app;
    }

}
