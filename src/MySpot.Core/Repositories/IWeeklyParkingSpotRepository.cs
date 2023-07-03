using MySpot.Api.Entities;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Repositories;

public interface IWeeklyParkingSpotRepository
{
    IEnumerable<WeeklyParkingSpot> GetAll();
    IEnumerable<WeeklyParkingSpot> GetByWeek(Week week) => throw new NotImplementedException();
    WeeklyParkingSpot? Get(ParkingSpotId id);
    void Add(WeeklyParkingSpot weeklyParkingSpot);
    void Update(WeeklyParkingSpot weeklyParkingSpot);
}