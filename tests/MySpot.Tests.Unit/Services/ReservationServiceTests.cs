using Moq;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;
using MySpot.Application.Commands.Handlers;
using MySpot.Application.Exceptions;
using MySpot.Core.Abstractions;
using MySpot.Core.DomainServices;
using MySpot.Core.Entities;
using MySpot.Core.Exceptions;
using MySpot.Core.Repositories;
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

        var command = new ReserveParkingSpotForVehicle(Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Guid.NewGuid(),
            Guid.NewGuid(), "ABC123", 1, _clock.Current().AddDays(1));

        var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));

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

        var command = new ReserveParkingSpotForVehicle(Guid.Parse("00000000-0000-0000-0000-000000000010"),
            Guid.NewGuid(),
            Guid.NewGuid(), "ABC123", 1, _clock.Current().AddDays(1));

        var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));
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

        var command = new ReserveParkingSpotForVehicle(Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Guid.NewGuid(),
            Guid.NewGuid(), "ABC123", 1, _clock.Current().AddDays(1));
        await _commandHandler.HandleAsync(command);

        _mockParkingReservationService.Setup(x => x.ReserveSpotForVehicle(
            It.IsAny<List<WeeklyParkingSpot>>(),
            It.IsAny<JobTitle>(),
            It.IsAny<WeeklyParkingSpot>(),
            It.IsAny<VehicleReservation>()
        )).Throws(new ParkingSpotAlreadyReservedException("P1", _clock.Current().AddDays(1)));

        var reservationId = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));

        reservationId.ShouldNotBeNull();
        reservationId.ShouldBeOfType<ParkingSpotAlreadyReservedException>();
    }

    #region ARRANGE

    private readonly IClock _clock;
    private readonly ICommandHandler<ReserveParkingSpotForVehicle> _commandHandler;
    private readonly Mock<IParkingReservationService> _mockParkingReservationService;
    private readonly Mock<IUserRepository> _mockUserRepository;

    public ReservationServiceTests()
    {
        _clock = new TestClock();
        var weeklyParkingSpotRepository = new InMemoryWeeklyParkingSpotRepository(_clock);
        _mockParkingReservationService = new Mock<IParkingReservationService>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockUserRepository.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
            .ReturnsAsync(new User(Guid.NewGuid(), new Email("test@abc.com"), new Username("Test123"), new Password("Qwerty123!"), new FullName("Test Name"), Role.User(), _clock.Current().AddDays(-2)));

        _commandHandler = new ReserveParkingSpotForVehicleHandler(weeklyParkingSpotRepository,
            _mockParkingReservationService.Object, _clock, _mockUserRepository.Object
        );
    }

    #endregion
}