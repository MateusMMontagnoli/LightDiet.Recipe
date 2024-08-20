using LightDiet.Recipe.Infra.Data.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LightDiet.Recipe.EndToEndTests.Common;

public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup>
    where TStartup : class
{

    protected override void ConfigureWebHost(
        IWebHostBuilder builder
    )
    {
        builder.ConfigureServices(services => {
            var dbOptions = services.FirstOrDefault(x => 
                x.ServiceType == typeof(
                    DbContextOptions<LightDietRecipeDbContext>
                )   
            );

            if (dbOptions is not null)
            {
                services.Remove(dbOptions);
            }

            services.AddDbContext<LightDietRecipeDbContext>(options =>
            {
                options.UseInMemoryDatabase("end2end-tests-db");
            });

        });

        base.ConfigureWebHost(builder);
    }
}
