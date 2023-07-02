using MySpot.Api.Entities;
using MySpot.Api.Exceptions;
using MySpot.Api.ValueObjects;
using Shouldly;

namespace MySpot.Tests.Unit.Entities;

public class WeeklyParkingSpotTests
{
    [Theory]
    [InlineData("2020-02-02")]
    [InlineData("2025-02-02")]
    [InlineData("2025-02-24")]
    public void given_invalid_date_add_reservation_should_fail(string dateString)
    {
        var invalidDate = DateTime.Parse(dateString);
        var reservation = new Reservation(Guid.NewGuid(), "John Doe", "ABC123", new Date(invalidDate));

        var exception = Record.Exception(() => _weeklyParkingSpot.AddReservation(reservation, _now));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidReservationDateException>();
    }
    
    [Fact]
    public void given_reservation_for_already_existing_date_add_reservation_should_fail()
    {
        var reservation = new Reservation(Guid.NewGuid(), "John Doe", "ABC123", _now.AddDays(1));
        _weeklyParkingSpot.AddReservation(reservation, _now);

        var exception = Record.Exception(() => _weeklyParkingSpot.AddReservation(reservation, _now));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ParkingSpotAlreadyReservedException>();
    }
    
    [Fact]
    public void given_reservation_for_not_taken_date_add_reservation_should_succeed()
    {
        var reservation = new Reservation(Guid.NewGuid(), "John Doe", "ABC123", _now.AddDays(1));
        
        _weeklyParkingSpot.AddReservation(reservation, _now);

        _weeklyParkingSpot.Reservations.ShouldContain(reservation);
        _weeklyParkingSpot.Reservations.ShouldHaveSingleItem();
    }

    #region ARRANGE

    private readonly WeeklyParkingSpot _weeklyParkingSpot;
    private readonly Date _now;

    public WeeklyParkingSpotTests()
    {
        _now = new Date(DateTime.Parse("2020-02-25"));
        _weeklyParkingSpot = new WeeklyParkingSpot(Guid.NewGuid(), new Week(_now), "P1");
    }

    #endregion
}