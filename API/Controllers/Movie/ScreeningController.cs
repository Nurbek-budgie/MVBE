using BLL.Interfaces.Movie;
using DTO.MovieDTOS;
using DTO.Reservation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Movie;

[ApiController]
public class ScreeningController : ControllerBase
{
    private readonly IScreeningService _screeningService;

    public ScreeningController(IScreeningService screeningService)
    {
        _screeningService = screeningService;
    }

    [HttpPost]
    [Route("/screening/create")]
    public async Task<ScreeningDto.Read> CreateScreening(ScreeningDto.Create screening)
    {
        return await _screeningService.CreateScreeningAsync(screening);
    }

    [HttpPut]
    [Route("/screening/update")]
    public async Task<ScreeningDto.Read> UpdateScreening(int id, ScreeningDto.Update screening)
    {
        return await _screeningService.UpdateScreeningAsync(id, screening);
    }

    [HttpGet]
    [Route("/screening/{id}")]
    public async Task<ActionResult<ScreeningDto.Read>> GetScreening(int id)
    {
        return await _screeningService.GetScreeningIdAsync(id);
    }
    
    [HttpGet]
    [Route("/screening/seats/{id}")]
    public async Task<ScreenSeatDto.Result> GetScreeningSeats(int id)
    {
        return await _screeningService.GetScreeningSeatsIdAsync(id);
    }

    
    [HttpGet]
    [Route("/screenings/active")]
    public async Task<IEnumerable<ScreeningDto.List>> GetAllActiveScreeningAsync()
    {
        return await _screeningService.GetAllActiveScreeningAsync();
    }
    
    [HttpGet]
    [Route("/screenings")]
    public async Task<IEnumerable<ScreeningDto.List>> GetAllScreeningAsync()
    {
        return await _screeningService.GetAllScreeningAsync();
    }

    [HttpDelete]
    [Route("/screenings/delete/{id}")]
    public async Task<bool> DeleteScreening(int id)
    {
        return await _screeningService.DeleteScreeningAsync(id);
    }
}