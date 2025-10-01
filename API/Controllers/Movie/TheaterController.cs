using API.Configurations;
using BLL.Interfaces.Movie;
using Common.Enums;
using DTO.MovieDTOS;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Movie;

[ApiController]
[Route("api/theaters")]
public class TheaterController : ControllerBase
{
    private readonly ITheaterService _theaterService;

    public TheaterController(ITheaterService theaterService)
    {
        _theaterService = theaterService;
    }

    // POST /api/theaters
    [AuthorizeRole(ERoles.Admin)]
    [HttpPost]
    public async Task<ActionResult<TheaterDto.Read>> CreateTheater([FromBody] TheaterDto.Create theater)
    {
        var created = await _theaterService.CreateAsync(theater);
        if (created == null) return BadRequest("Unable to create theater");
        return CreatedAtAction(nameof(GetTheater), new { id = created.Id }, created);
    }

    // PUT /api/theaters/{id}/logo
    [AuthorizeRole(ERoles.Admin)]
    [HttpPut("{id:int}/logo")]
    public async Task<ActionResult<TheaterDto.Read>> UploadLogo(int id, IFormFile logo)
    {
        var result = await _theaterService.UploadLogoImage(id, logo);
        if (result == null) return NotFound("Theater not found");
        return Ok(result);
    }

    // PUT /api/theaters/{id}
    [AuthorizeRole(ERoles.Admin)]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<TheaterDto.Read>> UpdateTheater(int id, [FromBody] TheaterDto.Update theater)
    {
        var result = await _theaterService.UpdateAsync(id, theater);
        if (result == null) return NotFound("Theater not found");
        return Ok(result);
    }

    // GET /api/theaters?active=true
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TheaterDto.List>>> GetAllTheaters([FromQuery] bool? active)
    {
        IEnumerable<TheaterDto.List> theaters;

        if (active == true)
        {
            theaters = await _theaterService.GetAllActiveTheatersAsync();
        }
        else
        {
            theaters = await _theaterService.GetAllTheatersAsync();
        }

        if (theaters == null || !theaters.Any()) return NotFound("No theaters found");
        return Ok(theaters);
    }

    // GET /api/theaters/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TheaterDto.Read>> GetTheater(int id)
    {
        var theater = await _theaterService.GetTheaterAsync(id);
        if (theater == null) return NotFound("Theater not found");
        return Ok(theater);
    }

    // DELETE /api/theaters/{id}
    [AuthorizeRole(ERoles.Admin)]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteTheater(int id)
    {
        var deleted = await _theaterService.DeleteTheater(id);
        if (!deleted) return NotFound("Theater not found");
        return NoContent();
    }
}