namespace MySpot.Api.Exceptions;

public sealed class InvalidReservationIdException : CustomException
{
    public Guid ReservationId { get; }
    public InvalidReservationIdException(Guid reservationId): base($"Invalid reservation id: {reservationId}")
    {
        ReservationId = reservationId;
    }
}