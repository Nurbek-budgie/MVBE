using BLL.Interfaces.Movie;
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

    [HttpPost]
    [Route("/screen/create")]
    public async Task<ScreenDto.Read> CreateScreen([FromQuery] ScreenDto.Create screenDto)
    {
        return await _screenService.CreateScreenAsync(screenDto);
    }
    
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
    
    [HttpGet]
    [Route("/screens/active")]
    public async Task<IEnumerable<ScreenDto.List>> GetActiveScreensAsync()
    {
        return await _screenService.GetAllActiveScreens();
    }

    [HttpDelete]
    [Route("/screens/{id}")]
    public async Task<bool> DeleteScreen(int id)
    {
        return await _screenService.DeleteScreenAsync(id);
    }
}