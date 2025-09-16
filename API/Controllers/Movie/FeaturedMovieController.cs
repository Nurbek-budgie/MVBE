using BLL.Interfaces.Movie;
using DTO.MovieDTOS;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Movie;

[ApiController]
public class FeaturedMovieController : ControllerBase
{
    private readonly IFeaturedMovieService _featuredMovieService;
    
    public FeaturedMovieController(IFeaturedMovieService  featuredMovieService)
    {
        _featuredMovieService = featuredMovieService;
    }

    [HttpPost]
    [Route("/api/FeaturedMovie/create")]
    public async Task<FeaturedMovieDto.Read> AddFeaturedMovie(FeaturedMovieDto.Create featuredMovieDto)
    {
        return await _featuredMovieService.AddFeaturedMovie(featuredMovieDto);
    }

    [HttpPut]
    [Route("/api/FeaturedMovie/change/{id}")]
    public async Task<FeaturedMovieDto.Read> ChangePositionMovie(int id, FeaturedMovieDto.Update featuredMovieDto)
    {
        return await _featuredMovieService.ChangePositionMovie(id, featuredMovieDto);
    }

    [HttpGet]
    [Route("/api/FeaturedMovies")]
    public async Task<IEnumerable<FeaturedMovieDto.Read>> ListOfFeaturedMovies()
    {
        return await _featuredMovieService.ListOfFeaturedMovies();
    }

    [HttpDelete]
    [Route("/api/FeaturedMovie/edit/{id}")]
    public async Task<bool> RemoveFeaturedMovie(int id)
    {
        return await _featuredMovieService.RemoveFeaturedMovie(id);
    }

    [HttpDelete]
    [Route("/api/FeaturedMovie/removeAll")]
    public async Task<bool> RemoveAllFeaturedMovies()
    {
        return await _featuredMovieService.RemoveAllFeaturedMovies();
    }
}