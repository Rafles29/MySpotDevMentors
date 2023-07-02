namespace MySpot.Api.ValueObjects;

public sealed record Week
{
    public Date From { get; }
    public Date To { get; }

    public Week(DateTimeOffset value)
    {
        var dayOfWeekNumber = (int)value.DayOfWeek;
        var pastDays = -1 * dayOfWeekNumber;
        var remainingDays = 7 - dayOfWeekNumber;
        From = new(value.AddDays(pastDays));
        To = new(value.AddDays(remainingDays));
    }

    public override string ToString() => $"{From} -> {To}";
}