using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Models;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("reservations")]
public class ReservationsController : ControllerBase
{
    private static readonly string[] ParkingSpotNames = { "P1", "P2", "P3", "P4", "P5" };

    private static readonly List<Reservation>
        Reservations = new();

    private static int Id = 1;

    [HttpGet("{id:int}", Name = "GetReservation")]
    public ActionResult<Reservation> GetReservation(int id)
    {
        var reservation = Reservations.FirstOrDefault(x => x.Id == id);
        return reservation is null ? NotFound() : Ok(reservation);
    }

    [HttpPost]
    public ActionResult Post([FromBody] Reservation reservation)
    {
        reservation.Id = Id;
        reservation.Date = DateTime.Now.AddDays(1).Date;

        if (ParkingSpotNames.All(x => x != reservation.ParkingSpotName))
            return BadRequest("Invalid parking spot name");

        var reservationAlreadyExists = Reservations.Any(x =>
            x.ParkingSpotName == reservation.ParkingSpotName && x.Date.Date == reservation.Date.Date);

        if (reservationAlreadyExists)
            return BadRequest("Parking spot already reserved");

        Id++;
        Reservations.Add(reservation);

        return CreatedAtAction(nameof(GetReservation), new { reservation.Id }, default);
    }

    [HttpGet]
    public ActionResult<ICollection<Reservation>> GetReservations()
    {
        return Ok(Reservations);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Reservation reservation)
    {
        var existingReservation = Reservations.SingleOrDefault(x => x.Id == id);

        if (existingReservation is null)
            return BadRequest($"The reservation with id ({id}) does not exist");

        existingReservation.LicencePlate = reservation.LicencePlate;
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var existingReservation = Reservations.SingleOrDefault(x => x.Id == id);

        if (existingReservation is null)
            return BadRequest($"The reservation with id ({id}) does not exist");

        Reservations.Remove(existingReservation);
        return NoContent();
    }
}