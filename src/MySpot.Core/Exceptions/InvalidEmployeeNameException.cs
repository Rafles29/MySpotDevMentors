namespace MySpot.Core.Exceptions;

public sealed class InvalidEmployeeNameException : CustomException
{
    public InvalidEmployeeNameException() : base("Name cannot be empty")
    {
        
    }
}