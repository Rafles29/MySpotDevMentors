using MySpot.Application.Commands;
using MySpot.Application.DTO;

namespace MySpot.Application.Services;

public interface IReservationService
{
    Task<IEnumerable<ReservationDto>> GetAllWeeklyAsync();
    Task<ReservationDto?> GetAsync(Guid id);
    Task ReserveForVehicleAsync(ReserveParkingSpotForVehicle command);
    Task ReserveForCleaningAsync(ReserveParkingSpotForCleaning command);
    Task ChangeReservationLicencePlateAsync(ChangeReservationLicencePlate command);
    Task DeleteAsync(DeleteReservation command);
}