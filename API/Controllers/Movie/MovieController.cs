using API.Configurations;
using BLL.Interfaces.Movie;
using Common.Enums;
using DTO.MovieDTOS;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Movie;

[ApiController]
[Route("api/movies")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    // POST /api/movies
    [AuthorizeRole(ERoles.Admin)]
    [HttpPost]
    public async Task<ActionResult<MovieDto.Read>> CreateMovie([FromBody]MovieDto.Create movieDto)
    {
        var movie = await _movieService.CreateMovieAsync(movieDto);

        if (movie == null) return BadRequest();
        
        return Ok(movie);
    }
    
    // GET /api/movies
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDto.List>>> GetAllMovies([FromQuery] bool? active)
    {
        if (!active.HasValue) return Ok(await _movieService.GetAllMoviesAsync());
        return Ok(await _movieService.GetActiveMoviesAsync());
    }

    // GET /api/movies/{id}
    [HttpGet]
    [Route("{id:int}/theaters")]
    public async Task<ActionResult<IEnumerable<CinemaDto.Theater>>> GetMovieGroupedByTheater(int id)
    {
        var movies = await _movieService.GetMovieGroupedByTheater(id);
        if (movies == null) return NotFound();
        return Ok(movies);
    }
    
    // GET /api/movies/{id}/screenings
    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<MovieDto.Read>> GetMovie(int id)
    {
        var movie = await _movieService.GetMovieByIdAsync(id);
        if (movie == null) return NotFound();
        return Ok(movie);
    }

    // GET /api/movies/{id}/theaters
    [HttpGet]
    [Route("{id:int}/screenings")]
    public async Task<ActionResult<MovieDto.ReadWithScreenings>> GetMovieWithScreenings(int id)
    {
        var movie = await _movieService.GetMovieByIdWithScreenings(id);
        
        if (movie == null) return NotFound();
        
        return Ok(movie);
    }
    
    // PUT /api/movies/{id}
    [AuthorizeRole(ERoles.Admin)]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<MovieDto.Read>> UpdateMovie(int id,[FromBody] MovieDto.Update movieDto)
    {
        var updated = await _movieService.UpdateMovieAsync(id, movieDto);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    // DELETE /api/movies/{id}
    [AuthorizeRole(ERoles.Admin)]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<bool>> DeleteMovie(int id)
    {
        var deleted = await _movieService.DeleteMovieAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}