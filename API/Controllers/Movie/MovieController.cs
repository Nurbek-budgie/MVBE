using BLL.Interfaces.Movie;
using DTO.MovieDTOS;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Movie;

[ApiController]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpPost]
    [Route("/api/createmovie")]
    public async Task<MovieDto.Read> CreateMovie(MovieDto.Create movieDto)
    {
        var movie = await _movieService.CreateMovieAsync(movieDto);
        
        return movie;
    }
    
    [HttpGet]
    [Route("/api/movies")]
    public async Task<IEnumerable<MovieDto.List>> GetAllMovies()
    {
        return await _movieService.GetAllMoviesAsync();
    }

    [HttpGet]
    [Route("/api/activemovies")]
    public async Task<IEnumerable<MovieDto.List>> GetActiveMovies()
    {
        return await _movieService.GetActiveMoviesAsync();
    }
    
    [HttpGet]
    [Route("/api/genremovies")]
    public async Task<IEnumerable<MovieDto.List>> GetMovieGenres(string genre)
    {
        return await _movieService.GetMoviesByGenreAsync(genre);
    }

    [HttpGet]
    [Route("/api/groupedmovies")]
    public async Task<IEnumerable<CinemaDto.Theater>> GetMOvv(int movieId)
    {
        return await _movieService.Getmov(movieId);
    }
    
    [HttpGet]
    [Route("/api/movies/{id}")]
    public async Task<MovieDto.Read> GetMovie(int id)
    {
        return await _movieService.GetMovieByIdAsync(id);
    }

    [HttpPut]
    [Route("/api/movieupdate")]
    public async Task<MovieDto.Read> UpdateMovie(int id, MovieDto.Update movieDto)
    {
        return await _movieService.UpdateMovieAsync(id, movieDto);
    }

    [HttpDelete]
    [Route("/api/moviedelete")]
    public async Task<bool> DeleteMovie(int id)
    {
        return await _movieService.DeleteMovieAsync(id);
    }
}