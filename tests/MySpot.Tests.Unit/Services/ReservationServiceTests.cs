using Moq;
using MySpot.Application.Commands;
using MySpot.Application.Exception;
using MySpot.Application.Services;
using MySpot.Core.Abstractions;
using MySpot.Core.DomainServices;
using MySpot.Core.Entities;
using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;
using MySpot.Infrastructure.DAL.Repositories;
using MySpot.Tests.Unit.Shared;
using Shouldly;

namespace MySpot.Tests.Unit.Services;

public class ReservationServiceTests
{
    [Fact]
    public async Task given_valid_command_reserve_for_vehicle_should_add_reservation()
    {
        _mockParkingReservationService.Setup(x => x.ReserveSpotForVehicle(
            It.IsAny<List<WeeklyParkingSpot>>(),
            It.IsAny<JobTitle>(),
            It.IsAny<WeeklyParkingSpot>(),
            It.IsAny<VehicleReservation>()
        ));
        
        var command = new ReserveParkingSpotForVehicle(Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.NewGuid(),
            "Joe Doe", "ABC123", 1, _clock.Current().AddDays(1));

        var exception = await Record.ExceptionAsync(() => _reservationService.ReserveForVehicleAsync(command));

        exception.ShouldBeNull();
    }

    [Fact]
    public async Task given_invalid_parking_spot_id_reserve_for_vehicle_should_fail()
    {
        _mockParkingReservationService.Setup(x => x.ReserveSpotForVehicle(
            It.IsAny<List<WeeklyParkingSpot>>(),
            It.IsAny<JobTitle>(),
            It.IsAny<WeeklyParkingSpot>(),
            It.IsAny<VehicleReservation>()
        ));
        
        var command = new ReserveParkingSpotForVehicle(Guid.Parse("00000000-0000-0000-0000-000000000010"), Guid.NewGuid(),
            "Joe Doe", "ABC123",1, _clock.Current().AddDays(1));

        var exception = await Record.ExceptionAsync(() => _reservationService.ReserveForVehicleAsync(command));
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<WeeklyParkingSpotNotFoundException>();
    }

    [Fact]
    public async Task given_reservation_for_already_taken_date_reserve_for_vehicle_should_fail()
    {
        _mockParkingReservationService.Setup(x => x.ReserveSpotForVehicle(
            It.IsAny<List<WeeklyParkingSpot>>(),
            It.IsAny<JobTitle>(),
            It.IsAny<WeeklyParkingSpot>(),
            It.IsAny<VehicleReservation>()
        ));
        
        var command = new ReserveParkingSpotForVehicle(Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.NewGuid(),
            "Joe Doe", "ABC123", 1,_clock.Current().AddDays(1));
        await _reservationService.ReserveForVehicleAsync(command);
        
        _mockParkingReservationService.Setup(x => x.ReserveSpotForVehicle(
            It.IsAny<List<WeeklyParkingSpot>>(),
            It.IsAny<JobTitle>(),
            It.IsAny<WeeklyParkingSpot>(),
            It.IsAny<VehicleReservation>()
        )).Throws(new ParkingSpotAlreadyReservedException("P1", _clock.Current().AddDays(1)));

        var reservationId = await Record.ExceptionAsync(() => _reservationService.ReserveForVehicleAsync(command));

        reservationId.ShouldNotBeNull();
        reservationId.ShouldBeOfType<ParkingSpotAlreadyReservedException>();
    }

    #region ARRANGE

    private readonly IClock _clock;
    private readonly ReservationService _reservationService;
    private readonly Mock<IParkingReservationService> _mockParkingReservationService;

    public ReservationServiceTests()
    {
        _clock = new TestClock();
        var weeklyParkingSpotRepository = new InMemoryWeeklyParkingSpotRepository(_clock);
        _mockParkingReservationService = new Mock<IParkingReservationService>();

        _reservationService = new ReservationService(
            _clock,
            weeklyParkingSpotRepository,
            _mockParkingReservationService.Object
        );
    }

    #endregion
}