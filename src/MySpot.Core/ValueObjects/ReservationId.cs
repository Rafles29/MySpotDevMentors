using MySpot.Core.Exceptions;

namespace MySpot.Core.ValueObjects;

public sealed record ReservationId
{
    public Guid Value { get; }
    
    public ReservationId(Guid value)
    {
        if(value == Guid.Empty)
            throw new InvalidReservationIdException(value);
        Value = value;
    }
    
    public static ReservationId Create() => new(Guid.NewGuid());
    
    public static implicit operator Guid(ReservationId reservationId) => reservationId.Value;
    
    public static implicit operator ReservationId(Guid value) => new(value);
    
    public override string ToString() => Value.ToString("N");
}