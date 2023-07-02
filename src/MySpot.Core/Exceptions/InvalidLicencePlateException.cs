namespace MySpot.Api.Exceptions;

public sealed class InvalidLicencePlateException : CustomException
{
    public string LicencePlate { get; }
    public InvalidLicencePlateException(string licencePlate): base($"Invalid licence plate: {licencePlate}")
    {
        LicencePlate = licencePlate;
    }
}