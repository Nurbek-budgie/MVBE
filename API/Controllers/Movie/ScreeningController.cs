using API.Configurations;
using BLL.Interfaces.Movie;
using Common.Enums;
using DTO.MovieDTOS;
using DTO.Reservation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Movie;

[ApiController]
[Route("api/screenings")]
public class ScreeningsController : ControllerBase
{
    private readonly IScreeningService _screeningService;

    public ScreeningsController(IScreeningService screeningService)
    {
        _screeningService = screeningService;
    }

    // POST /api/screenings
    [AuthorizeRole(ERoles.Admin, ERoles.Manager)]
    [HttpPost]
    public async Task<ActionResult<ScreeningDto.Read>> CreateScreening([FromBody] ScreeningDto.Create screening)
    {
        var created = await _screeningService.CreateScreeningAsync(screening);
        if (created == null) return BadRequest("Unable to create screening");
        return Ok(created);
    }

    // PUT /api/screenings/{id}
    [AuthorizeRole(ERoles.Admin, ERoles.Manager)]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ScreeningDto.Read>> UpdateScreening(int id, [FromBody] ScreeningDto.Update screening)
    {
        var updated = await _screeningService.UpdateScreeningAsync(id, screening);
        if (updated == null) return NotFound("Screening not found");
        return Ok(updated);
    }

    // GET /api/screenings/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ScreeningDto.Read>> GetScreening(int id)
    {
        var screening = await _screeningService.GetScreeningIdAsync(id);
        if (screening == null) return NotFound("Screening not found");
        return Ok(screening);
    }

    // GET /api/screenings/{id}/seats
    [HttpGet("{id:int}/seats")]
    public async Task<ActionResult<ScreenSeatDto.Result>> GetScreeningSeats(int id)
    {
        var seats = await _screeningService.GetScreeningSeatsIdAsync(id);
        if (seats == null) return NotFound("Seats not found for this screening");
        return Ok(seats);
    }

    // GET /api/screenings?active=true
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ScreeningDto.List>>> GetAllScreenings([FromQuery] bool? active)
    {
        if (active == true)
        {
            var activeScreenings = await _screeningService.GetAllActiveScreeningAsync();
            if (activeScreenings == null || !activeScreenings.Any()) return NotFound("No active screenings found");
            return Ok(activeScreenings);
        }

        var allScreenings = await _screeningService.GetAllScreeningAsync();
        if (allScreenings == null || !allScreenings.Any()) return NotFound("No screenings found");
        return Ok(allScreenings);
    }

    // DELETE /api/screenings/{id}
    [AuthorizeRole(ERoles.Admin, ERoles.Manager)]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteScreening(int id)
    {
        var deleted = await _screeningService.DeleteScreeningAsync(id);
        if (!deleted) return NotFound("Screening not found");
        return NoContent();
    }
}