using MySpot.ApApplicationi.Commands;
using MySpot.Api.Entities;
using MySpot.Api.Repositories;
using MySpot.Api.ValueObjects;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Exception;

namespace MySpot.Application.Services;

public class ReservationService : IReservationService
{
    private readonly IClock _clock;
    private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;

    public ReservationService(IClock clock, IWeeklyParkingSpotRepository weeklyParkingSpotRepository)
    {
        _clock = clock;
        _weeklyParkingSpotRepository = weeklyParkingSpotRepository;
    }

    public async Task<IEnumerable<ReservationDto>> GetAllWeeklyAsync() =>
        (await _weeklyParkingSpotRepository.GetAllAsync())
        .SelectMany(x => x.Reservations).Select(x =>
            new ReservationDto
            {
                Id = x.Id,
                EmployeeName = x.EmployeeName,
                Date = x.Date.Value.Date
            });

    public async Task<ReservationDto?> GetAsync(Guid id) => (await GetAllWeeklyAsync()).SingleOrDefault(x => x.Id == id);

    public async Task CreateAsync(CreateReservation command)
    {
        var (spotId, reservationId, employeeName, licencePlate, date) = command;

        var weeklyParkingSpot = await _weeklyParkingSpotRepository.GetAsync(spotId);

        if (weeklyParkingSpot is null)
        {
            throw new WeeklyParkingSpotNotFoundException(reservationId);
        }

        var reservation = new Reservation(reservationId, employeeName, licencePlate, new Date(date));

        weeklyParkingSpot.AddReservation(reservation, CurrentDate());
        await _weeklyParkingSpotRepository.UpdateAsync(weeklyParkingSpot);
    }

    public async Task UpdateAsync(ChangeReservationLicencePlate command)
    {
        var weeklyParkingSpot = await GetWeeklyParkingSpotByReservation(command.ReservationId);

        if (weeklyParkingSpot is null)
        {
            throw new WeeklyParkingSpotNotFoundException();
        }

        var reservationId = new ReservationId(command.ReservationId);
        var reservation = weeklyParkingSpot.Reservations.SingleOrDefault(x => x.Id == reservationId);

        if (reservation is null)
        {
            throw new ReservationNotFoundException(command.ReservationId);
        }

        reservation.ChangeLicencePlate(command.LicencePlate);
        await _weeklyParkingSpotRepository.UpdateAsync(weeklyParkingSpot);
    }

    public async Task DeleteAsync(DeleteReservation command)
    {
        var weeklyParkingSpot = await GetWeeklyParkingSpotByReservation(command.ReservationId);

        if (weeklyParkingSpot is null)
        {
            throw new WeeklyParkingSpotNotFoundException(command.ReservationId);
        }

        weeklyParkingSpot.RemoveReservation(command.ReservationId);
        await _weeklyParkingSpotRepository.UpdateAsync(weeklyParkingSpot);
    }

    private async Task<WeeklyParkingSpot?> GetWeeklyParkingSpotByReservation(ReservationId id) =>
        (await _weeklyParkingSpotRepository.GetAllAsync())
            .SingleOrDefault(x => x.Reservations.Any(reservation => reservation.Id == id));

    private DateTimeOffset CurrentDate() => _clock.Current();
}