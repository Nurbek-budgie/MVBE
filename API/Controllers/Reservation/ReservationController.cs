using System.Security.Claims;
using API.Configurations;
using BLL.Interfaces.Reservation;
using Common.Enums;
using DTO.Reservation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Reservation;

[ApiController]
[Route("api/reservations")]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _service;

    public ReservationController(IReservationService service)
    {
        _service = service;
    }

    // POST /api/reservations
    [AuthorizeRole(ERoles.Audience, ERoles.Client, ERoles.Admin)]
    [HttpPost]
    public async Task<ActionResult<ReservationDto.Read>> CreateReservation([FromBody] ReservationDto.ReserveSeat reservation)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var reserve = await _service.CreateReservation(userId, reservation);

        if (reserve == null) return BadRequest("Unable to create reservation");
        return Ok(reserve);
    }
}
