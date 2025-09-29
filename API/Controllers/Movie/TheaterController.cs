using API.Configurations;
using BLL.Interfaces.Movie;
using Common.Enums;
using DTO.MovieDTOS;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Movie;

[ApiController]
public class TheaterController : ControllerBase
{
    private readonly ITheaterService _theaterService;

    public TheaterController(ITheaterService theaterService)
    {
        _theaterService = theaterService;
    }

    [AuthorizeRole(ERoles.Admin)]
    [HttpPost]
    [Route("/api/theater/add")]
    public async Task<TheaterDto.Read> CreateTheater(TheaterDto.Create theater)
    {
        return await _theaterService.CreateAsync(theater);
    }

    [AuthorizeRole(ERoles.Admin)]
    [HttpPut]
    [Route("/api/theater/update/logo")]
    public async Task<TheaterDto.Read> UploadLogo(int id, IFormFile logo)
    {
        return await _theaterService.UploadLogoImage(id, logo);
    }
    
    [AuthorizeRole(ERoles.Admin)]
    [HttpPut]
    [Route("/api/theater/update")]
    public async Task<TheaterDto.Read> UpdateTheater(int id, TheaterDto.Update theater)
    {
        return await _theaterService.UpdateAsync(id, theater);
    }

    [HttpGet]
    [Route("/api/theaters")]
    public async Task<IEnumerable<TheaterDto.List>> GetAllTheaters()
    {
        return await _theaterService.GetAllTheatersAsync();
    }

    [HttpGet]
    [Route("/api/theaters/active")]
    public async Task<IEnumerable<TheaterDto.List>> GetAllActiveTheaters()
    {
        return await _theaterService.GetAllActiveTheatersAsync();
    }

    [HttpGet]
    [Route("/api/theater/{id}")]
    public async Task<TheaterDto.Read> Get(int id)
    {
        return await _theaterService.GetTheaterAsync(id);
    }

    [AuthorizeRole(ERoles.Admin)]
    [HttpDelete]
    [Route("/api/theater/delete")]
    public async Task<bool> DeleteTheater(int id)
    {
        return await _theaterService.DeleteTheater(id);
    }
}