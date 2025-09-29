using API.Configurations;
using BLL.Interfaces.Movie;
using Common.Enums;
using Common.Enums.MovieEnums;
using DTO.MovieDTOS;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Movie;

[ApiController]
public class ScreenController : ControllerBase
{
    private readonly IScreenService _screenService;
    
    public ScreenController(IScreenService screenService)
    {
        _screenService = screenService;
    }

    [AuthorizeRole(ERoles.Admin, ERoles.Manager)]
    [HttpPost]
    [Route("/screen/create")]
    public async Task<ScreenDto.Read> CreateScreen([FromQuery] ScreenDto.Create screenDto)
    {
        return await _screenService.CreateScreenAsync(screenDto);
    }
    
    [AuthorizeRole(ERoles.Admin, ERoles.Manager)]
    [HttpPut]
    [Route("/screen/update")]
    public async Task<ScreenDto.Read> UpdateScreen([FromQuery] int id,[FromQuery] ScreenDto.Update screenDto)
    {
        return await _screenService.UpdateScreenAsync(id, screenDto);
    }
    
    [HttpGet]
    [Route("/screen/{id}")]
    public async Task<ScreenDto.Read> GetScreenIdAsync(int id)
    {
        return await _screenService.GetScreenByIdAsync(id);
    }
    
    [HttpGet]
    [Route("/screens")]
    public async Task<IEnumerable<ScreenDto.List>> GetScreensAsync()
    {
        return await _screenService.GetAllScreens();
    }
    
    [AuthorizeRole(ERoles.Admin, ERoles.Manager)]
    [HttpGet]
    [Route("/screen/theatershowtimd")]
    public async Task<IEnumerable<ScreenDto.Screen>> GetScreensByTheaterShowtimesIdAsync([FromQuery] int? theaterId = null)
    {
        // If user is a Manager, override theaterId from JWT claim
        if (User.IsInRole(ERoles.Manager.ToString()))
        {
            var theaterClaim = User.FindFirst("theaterId")?.Value;
            if (theaterClaim == null)
                throw new UnauthorizedAccessException("No theater assigned to this manager.");

            theaterId = int.Parse(theaterClaim);
        }

        if (!theaterId.HasValue)
            throw new ArgumentException("TheaterId is required for Admins.");

        return await _screenService.GetScreenbyTheaterShowtimesIdAsync(theaterId.Value);
    }
    
    [AuthorizeRole(ERoles.Admin, ERoles.Manager)]
    [HttpGet]
    [Route("/screen/theater")]
    public async Task<IEnumerable<ScreenDto.Read>> GetScreensByTheaterIdAsync([FromQuery] int? theaterId = null)
    {
        // If user is a Manager, override theaterId from JWT claim
        if (User.IsInRole(ERoles.Manager.ToString()))
        {
            var theaterClaim = User.FindFirst("theaterId")?.Value;
            if (theaterClaim == null)
                throw new UnauthorizedAccessException("No theater assigned to this manager.");

            theaterId = int.Parse(theaterClaim);
        }

        if (!theaterId.HasValue)
            throw new ArgumentException("TheaterId is required for Admins.");
        
        return await _screenService.GetScreenByTheaterIdAsync(theaterId.Value);
    }
    
    [HttpGet]
    [Route("/screens/type/{type}")]
    public async Task<IEnumerable<ScreenTypeDto.Cinema>> GetScreenTypeAsync(ScreenType type)
    {
        return await _screenService.GetScreenTypeTheaterAsync(type);
    }
    
    [HttpGet]
    [Route("/screens/active")]
    public async Task<IEnumerable<ScreenDto.List>> GetActiveScreensAsync()
    {
        return await _screenService.GetAllActiveScreens();
    }

    [AuthorizeRole(ERoles.Admin, ERoles.Manager)]
    [HttpDelete]
    [Route("/screens/{id}")]
    public async Task<bool> DeleteScreen(int id)
    {
        return await _screenService.DeleteScreenAsync(id);
    }
}