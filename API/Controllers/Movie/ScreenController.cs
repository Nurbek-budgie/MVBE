using API.Configurations;
using BLL.Interfaces.Movie;
using Common.Enums;
using Common.Enums.MovieEnums;
using DTO.MovieDTOS;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Movie;

[ApiController]
[Route("api/screens")]
public class ScreenController : ControllerBase
{
    private readonly IScreenService _screenService;
    
    public ScreenController(IScreenService screenService)
    {
        _screenService = screenService;
    }

    // POST /api/screen
    [AuthorizeRole(ERoles.Admin, ERoles.Manager)]
    [HttpPost]
    public async Task<ActionResult<ScreenDto.Read>> CreateScreen([FromBody] ScreenDto.Create screenDto)
    {
        var result = await _screenService.CreateScreenAsync(screenDto);
        if (result == null) return BadRequest("Unable to create screen");
        return Ok(result);
    }
    
    // PUT /api/screen/{id}
    [AuthorizeRole(ERoles.Admin, ERoles.Manager)]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ScreenDto.Read>> UpdateScreen(int id, [FromBody] ScreenDto.Update screenDto)
    {
        var result = await _screenService.UpdateScreenAsync(id, screenDto);
        if (result == null) return NotFound("Screen not found");
        return Ok(result);
    }
    
    // GET /api/screen/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ScreenDto.Read>> GetScreenIdAsync(int id)
    {
        var result = await _screenService.GetScreenByIdAsync(id);
        if (result == null) return NotFound("Screen not found");
        return Ok(result);
    }
    
    // GET /api/screen?active=true
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ScreenDto.List>>> GetScreensAsync([FromQuery] bool? active)
    {
        if (!active.HasValue)
        {
            var result = await _screenService.GetAllScreens();
            if (result == null || !result.Any()) return NotFound("No screens found");
            return Ok(result);
        }
        
        var activeResult = await _screenService.GetAllActiveScreens();
        if (activeResult == null || !activeResult.Any()) return NotFound("No active screens found");
        return Ok(activeResult);
    }
    
    // GET /api/screen/theater-screens
    [AuthorizeRole(ERoles.Admin, ERoles.Manager)]
    [HttpGet("theater-screens")]
    public async Task<ActionResult<IEnumerable<ScreenDto.Screen>>> GetScreensByTheaterShowtimesIdAsync([FromQuery] int? theaterId = null)
    {
        if (User.IsInRole(ERoles.Manager.ToString()))
        {
            var theaterClaim = User.FindFirst("theaterId")?.Value;
            if (theaterClaim == null) return Unauthorized("No theater assigned to this manager.");

            theaterId = int.Parse(theaterClaim);
        }

        if (!theaterId.HasValue)
            return BadRequest("TheaterId is required for Admins.");

        var result = await _screenService.GetScreenbyTheaterShowtimesIdAsync(theaterId.Value);
        if (result == null || !result.Any()) return NotFound("No screens found");
        return Ok(result);
    }
    
    // GET /api/screen/theater
    [AuthorizeRole(ERoles.Admin, ERoles.Manager)]
    [HttpGet("theater")]
    public async Task<ActionResult<IEnumerable<ScreenDto.Read>>> GetScreensByTheaterIdAsync([FromQuery] int? theaterId = null)
    {
        if (User.IsInRole(ERoles.Manager.ToString()))
        {
            var theaterClaim = User.FindFirst("theaterId")?.Value;
            if (theaterClaim == null) return Unauthorized("No theater assigned to this manager.");

            theaterId = int.Parse(theaterClaim);
        }

        if (!theaterId.HasValue)
            return BadRequest("TheaterId is required for Admins.");

        var result = await _screenService.GetScreenByTheaterIdAsync(theaterId.Value);
        if (result == null || !result.Any()) return NotFound("No screens found");
        return Ok(result);
    }
    
    // GET /api/screen/type/{type}
    [HttpGet("type/{type}")]
    public async Task<ActionResult<IEnumerable<ScreenTypeDto.Cinema>>> GetScreenTypeAsync(ScreenType type)
    {
        var result = await _screenService.GetScreenTypeTheaterAsync(type);
        if (result == null || !result.Any()) return NotFound("No screens of this type found");
        return Ok(result);
    }

    // DELETE /api/screen/{id}
    [AuthorizeRole(ERoles.Admin, ERoles.Manager)]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteScreen(int id)
    {
        var result = await _screenService.DeleteScreenAsync(id);
        if (!result) return NotFound("Screen not found");
        return NoContent();
    }
}