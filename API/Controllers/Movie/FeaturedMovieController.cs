using API.Configurations;
using BLL.Interfaces.Movie;
using Common.Enums;
using DTO.MovieDTOS;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Movie;

[ApiController]
[Route("api/featuredmovies")]
public class FeaturedMovieController : ControllerBase
{
    private readonly IFeaturedMovieService _featuredMovieService;
    private readonly ILogger<FeaturedMovieController> _logger;
    
    public FeaturedMovieController(IFeaturedMovieService  featuredMovieService,  ILogger<FeaturedMovieController> logger)
    {
        _featuredMovieService = featuredMovieService;
        _logger = logger;
    }

    /// <summary>
    /// Add a new featured movie.
    /// </summary>
    // POST /api/featuredMovie
    [AuthorizeRole(ERoles.Admin)]
    [HttpPost]
    public async Task<ActionResult<FeaturedMovieDto.Read>> AddFeaturedMovie( [FromBody] FeaturedMovieDto.Create featuredMovieDto)
    {
        _logger.LogInformation("Adding a new featured movie: {featuredMovie}", featuredMovieDto);
        try
        {
            var result = await _featuredMovieService.AddFeaturedMovie(featuredMovieDto);
            if (result == null)
            {
                _logger.LogWarning("Could not add featured movie: {featuredMovie}", featuredMovieDto);
                return BadRequest("Could not add featured movie.");
            }    
            
            _logger.LogInformation("Successfully added: {featuredMovie}", featuredMovieDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding featured movie: {featuredMovie}", featuredMovieDto);
            return StatusCode(500);
        }
        
    }

    /// <summary>
    /// Change movie position or order
    /// </summary>
    /// <param name="id"></param>
    /// <param name="featuredMovieDto"></param>
    /// <returns></returns>
    [AuthorizeRole(ERoles.Admin)]
    [HttpPut("{id:int}/position")]
    public async Task<ActionResult<FeaturedMovieDto.Read>> ChangePositionMovie(int id, [FromBody] FeaturedMovieDto.Update featuredMovieDto)
    {
        _logger.LogInformation("Changing position movie: {featuredMovie}", featuredMovieDto);

        try
        {
            var result = await _featuredMovieService.ChangePositionMovie(id, featuredMovieDto);
            if (result == null)
            {
                _logger.LogWarning("Could not change position movie: {featuredMovie}", featuredMovieDto);
                return NotFound();
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing position movie: {featuredMovie}", featuredMovieDto);
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Lists of featured movies
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FeaturedMovieDto.Read>>> ListOfFeaturedMovies()
    {
        _logger.LogInformation("Getting all Featured Movies");

        try
        {
            var result = await _featuredMovieService.ListOfFeaturedMovies();
            if (result == null || !result.Any())
            {
                _logger.LogWarning("Could not get all Featured Movies");
                return NotFound();
            }
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting all Featured Movies");
            return StatusCode(500);
        }
    }

    /// <summary>
    /// remove featured movie
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AuthorizeRole(ERoles.Admin)]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> RemoveFeaturedMovie(int id)
    {
        _logger.LogInformation("Removing Featured Movie");

        try
        {
            var result = await _featuredMovieService.RemoveFeaturedMovie(id);
            if (result == null)
            {
                _logger.LogWarning("Could not remove Featured Movie");
                return NotFound();
            }
            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error Removing Featured Movie");
            return StatusCode(500);
        }
        
    }

    /// <summary>
    /// Removes all featured movies
    /// </summary>
    /// <returns></returns>
    [AuthorizeRole(ERoles.Admin)]
    [HttpDelete("removeAll")]
    public async Task<ActionResult> RemoveAllFeaturedMovies()
    {
        _logger.LogInformation("Removing all Featured Movies");

        try
        {
            var result = await _featuredMovieService.RemoveAllFeaturedMovies();
            if (!result)
            {
                _logger.LogWarning("Could not remove all Featured Movies");
                return BadRequest("Failed to remove all featured movies.");
            }
            
            return NoContent();    
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error Removing all Featured Movies");
            return StatusCode(500);
        }
    }
}