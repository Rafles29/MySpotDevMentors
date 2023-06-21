﻿using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Commands;
using MySpot.Api.DTO;
using MySpot.Api.Services;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("reservations")]
public class ReservationsController : ControllerBase
{
    private readonly ReservationService _service = new();

    [HttpGet]
    public ActionResult<ICollection<ReservationDto>> GetAll()
    {
        return Ok(_service.GetAllWeekly());
    }

    [HttpGet("{id:guid}", Name = "GetReservation")]
    public ActionResult<ReservationDto> GetReservation(Guid id)
    {
        var reservation = _service.Get(id);
        return reservation is null ? NotFound() : Ok(reservation);
    }

    [HttpPost]
    public ActionResult Post([FromBody] CreateReservation command)
    {
        var id = _service.Create(command with { ReservationId = Guid.NewGuid() });

        if (id is null)
            return BadRequest();

        return CreatedAtAction(nameof(GetReservation), new { id }, default);
    }


    [HttpPut("{id:guid}")]
    public ActionResult Put(Guid id, ChangeReservationLicencePlate command)
    {
        var succeeded = _service.Update(command with { ReservationId = id });

        if (!succeeded)
            return BadRequest($"The reservation with id ({id}) does not exist");

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public ActionResult Delete(Guid id)
    {
        var succeeded = _service.Delete(new DeleteReservation(ReservationId: id));

        if (!succeeded)
            return BadRequest($"The reservation with id ({id}) does not exist");

        return NoContent();
    }
}