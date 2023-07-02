namespace MySpot.Api.Exceptions;

public sealed class InvalidParkingSpotIdException : CustomException
{
    public Guid ParkingSpotId { get; }
    public InvalidParkingSpotIdException(Guid parkingSpotId): base($"Invalid parking spot id: {parkingSpotId}")
    {
        ParkingSpotId = parkingSpotId;
    }
}