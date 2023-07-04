using MySpot.Core.Exceptions;

namespace MySpot.Application.Exception;

public class ReservationNotFoundException : CustomException
{
    private Guid Id { get; }

    public ReservationNotFoundException(Guid id) : base($"Reservation with id: '{id}' was not found.")
    {
        Id = id;
    }
}