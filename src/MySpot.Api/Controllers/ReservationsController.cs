using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Models;
using MySpot.Api.Services;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("reservations")]
public class ReservationsController : ControllerBase
{
    private readonly ReservationService _service = new();
    
    [HttpGet]
    public ActionResult<ICollection<Reservation>> GetAll()
    {
        return Ok(_service.GetAll());
    }
    
    [HttpGet("{id:int}", Name = "GetReservation")]
    public ActionResult<Reservation> GetReservation(int id)
    {
        var reservation = _service.Get(id);
        return reservation is null ? NotFound() : Ok(reservation);
    }

    [HttpPost]
    public ActionResult Post([FromBody] Reservation reservation)
    {
        var id = _service.Create(reservation);
        if (id is null)
            return BadRequest();

        return CreatedAtAction(nameof(GetReservation), new { id }, default);
    }
    

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Reservation reservation)
    {
        reservation.Id = id;
        var succeeded = _service.Update(reservation);

        if (!succeeded)
            return BadRequest($"The reservation with id ({id}) does not exist");

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var succeeded = _service.Delete(id);

        if (!succeeded)
            return BadRequest($"The reservation with id ({id}) does not exist");

        return NoContent();
    }
}