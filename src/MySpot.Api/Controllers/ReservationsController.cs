using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Commands;
using MySpot.Api.DTO;
using MySpot.Api.Entities;
using MySpot.Api.Services;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("reservations")]
public class ReservationsController : ControllerBase
{
    private static readonly ReservationService Service = new(new Clock(), new List<WeeklyParkingSpot>()
    {
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000001"), new Week(new Clock().Current()),
            "P1"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000002"), new Week(new Clock().Current()),
            "P2"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000003"), new Week(new Clock().Current()),
            "P3"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000004"), new Week(new Clock().Current()),
            "P4"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000005"), new Week(new Clock().Current()),
            "P5"),
    });

    [HttpGet]
    public ActionResult<ICollection<ReservationDto>> GetAll()
    {
        return Ok(Service.GetAllWeekly());
    }

    [HttpGet("{id:guid}", Name = "GetReservation")]
    public ActionResult<ReservationDto> GetReservation(Guid id)
    {
        var reservation = Service.Get(id);
        return reservation is null ? NotFound() : Ok(reservation);
    }

    [HttpPost]
    public ActionResult Post([FromBody] CreateReservation command)
    {
        var id = Service.Create(command with { ReservationId = Guid.NewGuid() });

        if (id is null)
            return BadRequest();

        return CreatedAtAction(nameof(GetReservation), new { id }, default);
    }


    [HttpPut("{id:guid}")]
    public ActionResult Put(Guid id, ChangeReservationLicencePlate command)
    {
        var succeeded = Service.Update(command with { ReservationId = id });

        if (!succeeded)
            return BadRequest($"The reservation with id ({id}) does not exist");

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public ActionResult Delete(Guid id)
    {
        var succeeded = Service.Delete(new DeleteReservation(ReservationId: id));

        if (!succeeded)
            return BadRequest($"The reservation with id ({id}) does not exist");

        return NoContent();
    }
}