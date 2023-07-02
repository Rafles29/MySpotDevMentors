using MySpot.Api.Entities;
using MySpot.Api.Repositories;
using MySpot.Api.ValueObjects;
using MySpot.Application.Services;

namespace MySpot.Infrastructure.Repositories;

internal sealed class InMemoryWeeklyParkingSpotRepository : IWeeklyParkingSpotRepository
{
    private readonly List<WeeklyParkingSpot> _weeklyParkingSpots;

    public InMemoryWeeklyParkingSpotRepository(IClock clock)
    {
        _weeklyParkingSpots = new()
        {
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000001"), new Week(clock.Current()),
                "P1"),
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000002"), new Week(clock.Current()),
                "P2"),
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000003"), new Week(clock.Current()),
                "P3"),
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000004"), new Week(clock.Current()),
                "P4"),
            new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000005"), new Week(clock.Current()),
                "P5"),
        };
    }
    
    public IEnumerable<WeeklyParkingSpot> GetAll() => _weeklyParkingSpots;

    public WeeklyParkingSpot? Get(ParkingSpotId id) => _weeklyParkingSpots.SingleOrDefault(x => x.Id == id);

    public void Add(WeeklyParkingSpot weeklyParkingSpot) => _weeklyParkingSpots.Add(weeklyParkingSpot);

    public void Update(WeeklyParkingSpot weeklyParkingSpot)
    {
    }
}