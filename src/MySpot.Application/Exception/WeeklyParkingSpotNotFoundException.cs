using MySpot.Core.Exceptions;

namespace MySpot.Application.Exception;

public class WeeklyParkingSpotNotFoundException : CustomException
{
    private Guid Id { get; }

    public WeeklyParkingSpotNotFoundException() : base("Weekly parking spot was not found.")
    {
    }

    public WeeklyParkingSpotNotFoundException(Guid id) : base($"Weekly parking spot with id: '{id}' was not found.")
    {
        Id = id;
    }
}