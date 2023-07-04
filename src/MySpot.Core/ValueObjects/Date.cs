namespace MySpot.Core.ValueObjects;

public sealed record Date(DateTimeOffset Value)
{
    public Date AddDays(int days) => new(Value.AddDays(days));
    
    public static implicit operator DateTimeOffset(Date date) => date.Value;
    
    public static implicit operator Date(DateTimeOffset value) => new(value);
    
    public static bool operator <(Date left, Date right) => left.Value < right.Value;
    
    public static bool operator >(Date left, Date right) => left.Value > right.Value;
    
    public static bool operator <=(Date left, Date right) => left.Value <= right.Value;
    
    public static bool operator >=(Date left, Date right) => left.Value >= right.Value;
    
    public static Date Now => new(DateTimeOffset.Now);

    public override string ToString() => Value.ToString("d");
}