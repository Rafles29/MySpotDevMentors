using MySpot.ApApplicationi.Commands;
using MySpot.Application.Commands;
using MySpot.Application.DTO;

namespace MySpot.Application.Services;

public interface IReservationService
{
    IEnumerable<ReservationDto> GetAllWeekly();
    ReservationDto? Get(Guid id);
    Guid? Create(CreateReservation command);
    bool Update(ChangeReservationLicencePlate command);
    bool Delete(DeleteReservation command);
}