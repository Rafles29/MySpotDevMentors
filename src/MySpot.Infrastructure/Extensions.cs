using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Api.Repositories;
using MySpot.Application.Services;
using MySpot.Infrastructure.Repositories;
using MySpot.Infrastructure.Time;

[assembly: InternalsVisibleTo("MySpot.Tests.Unit")]
namespace MySpot.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services
                .AddSingleton<IWeeklyParkingSpotRepository, InMemoryWeeklyParkingSpotRepository>()
                .AddSingleton<IClock, Clock>();
    }
}