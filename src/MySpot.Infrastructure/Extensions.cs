using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Services;
using MySpot.Infrastructure.DAL;
using MySpot.Infrastructure.Time;

[assembly: InternalsVisibleTo("MySpot.Tests.Unit")]
namespace MySpot.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // var appOptions = configuration.AddOptions<AppOptions>("App");
        services.Configure<AppOptions>(configuration.GetRequiredSection("App"));
        
        return services
            .AddPostgres(configuration)
            // .AddSingleton<IWeeklyParkingSpotRepository, InMemoryWeeklyParkingSpotRepository>()
            .AddSingleton<IClock, Clock>();
    }

    public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        var options = new T();
        var section = configuration.GetRequiredSection(sectionName);
        section.Bind(options);

        return options;
    }
}