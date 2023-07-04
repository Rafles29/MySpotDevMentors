using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Services;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("reservations")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationsController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet]
    public async Task<ActionResult<ReservationDto[]>> GetAll() => Ok(await _reservationService.GetAllWeeklyAsync());

    [HttpGet("{id:guid}", Name = "GetReservation")]
    public async Task<ActionResult<ReservationDto>> GetReservation(Guid id)
    {
        var reservation = await _reservationService.GetAsync(id);
        return reservation is null ? NotFound() : Ok(reservation);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CreateReservation command)
    {
        command = command with { ReservationId = Guid.NewGuid() };
        await _reservationService.CreateAsync(command);
        return CreatedAtAction(nameof(GetReservation), new { id = command.ReservationId }, default);
    }


    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Put(Guid id, ChangeReservationLicencePlate command)
    {
        await _reservationService.UpdateAsync(command with { ReservationId = id });
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _reservationService.DeleteAsync(new DeleteReservation(id));
        return NoContent();
    }
}