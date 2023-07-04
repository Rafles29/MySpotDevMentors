namespace MySpot.Core.Exceptions;

public sealed class InvalidCapacityException : CustomException
{
    public int Capacity { get; }

    public InvalidCapacityException(int capacity) : base($"Invalid capacity: {capacity}")
    {
        Capacity = capacity;
    }
}