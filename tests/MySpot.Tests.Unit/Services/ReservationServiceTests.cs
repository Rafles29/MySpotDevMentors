using MySpot.Api.Exceptions;
using MySpot.Application.Commands;
using MySpot.Application.Services;
using MySpot.Infrastructure.Repositories;
using MySpot.Tests.Unit.Shared;
using Shouldly;

namespace MySpot.Tests.Unit.Services;

public class ReservationServiceTests
{
    [Fact]
    public void given_valid_command_create_should_add_reservation()
    {
        var command = new CreateReservation(Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.NewGuid(),
            "Joe Doe", "ABC123", _clock.Current().AddDays(1));

        var reservationId = _reservationService.Create(command);

        reservationId.ShouldNotBeNull();
        reservationId.Value.ShouldBe(command.ReservationId);
    }

    [Fact]
    public void given_invalid_parking_spot_id_create_should_fail()
    {
        var command = new CreateReservation(Guid.Parse("00000000-0000-0000-0000-000000000010"), Guid.NewGuid(),
            "Joe Doe", "ABC123", _clock.Current().AddDays(1));

        var reservationId = _reservationService.Create(command);

        reservationId.ShouldBeNull();
    }

    [Fact]
    public void given_reservation_for_already_taken_date_create_should_fail()
    {
        var command = new CreateReservation(Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.NewGuid(),
            "Joe Doe", "ABC123", _clock.Current().AddDays(1));
        _reservationService.Create(command);

        var reservationId = Record.Exception(() => _reservationService.Create(command));

        reservationId.ShouldNotBeNull();
        reservationId.ShouldBeOfType<ParkingSpotAlreadyReservedException>();
    }

    #region ARRANGE

    private readonly IClock _clock;
    private readonly ReservationService _reservationService;

    public ReservationServiceTests()
    {
        _clock = new TestClock();
        var weeklyParkingSpotRepository = new InMemoryWeeklyParkingSpotRepository(_clock);
        _reservationService = new ReservationService(_clock, weeklyParkingSpotRepository);
    }

    #endregion
}