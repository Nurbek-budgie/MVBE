using BLL.Interfaces.Reservation;
using DTO.Reservation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Reservation;

[ApiController]
[Route("api/reservations")] // /api/reservations
public class ReservationController : ControllerBase
{
    private readonly IReservationService _service;

    public ReservationController(IReservationService service)
    {
        _service = service;
    }
    
    // POST /api/reservations
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<ReservationDto.Read>> CreateReservation([FromBody] ReservationDto.ReserveSeat reservation)
    {
        // TODO: replace hard-coded GUID with user ID from JWT or other source
        var userId = Guid.Parse("0198b968-a243-749f-995e-567c645081a8"); 
        var reserve = await _service.CreateReservation(userId, reservation);

        if (reserve == null) return BadRequest("Unable to create reservation");
        return Ok(reserve);
    }
    // GET /api/reservations/{id}
    // DELETE /api/reservations/{id}
}