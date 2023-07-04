using MySpot.Application.Commands;
using MySpot.Application.Exception;
using MySpot.Application.Services;
using MySpot.Core.Exceptions;
using MySpot.Infrastructure.DAL.Repositories;
using MySpot.Tests.Unit.Shared;
using Shouldly;

namespace MySpot.Tests.Unit.Services;

public class ReservationServiceTests
{
    [Fact]
    public async Task given_valid_command_create_should_add_reservation()
    {
        var command = new CreateReservation(Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.NewGuid(),
            "Joe Doe", "ABC123", _clock.Current().AddDays(1));

        var exception = await Record.ExceptionAsync(() => _reservationService.CreateAsync(command));

        exception.ShouldBeNull();
    }

    [Fact]
    public async Task given_invalid_parking_spot_id_create_should_fail()
    {
        var command = new CreateReservation(Guid.Parse("00000000-0000-0000-0000-000000000010"), Guid.NewGuid(),
            "Joe Doe", "ABC123", _clock.Current().AddDays(1));

        var exception = await Record.ExceptionAsync(() => _reservationService.CreateAsync(command));
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<WeeklyParkingSpotNotFoundException>();
    }

    [Fact]
    public async Task given_reservation_for_already_taken_date_create_should_fail()
    {
        var command = new CreateReservation(Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.NewGuid(),
            "Joe Doe", "ABC123", _clock.Current().AddDays(1));
        await _reservationService.CreateAsync(command);

        var reservationId = await Record.ExceptionAsync( () => _reservationService.CreateAsync(command));

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