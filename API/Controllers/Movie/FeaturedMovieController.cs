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
    
    public FeaturedMovieController(IFeaturedMovieService  featuredMovieService)
    {
        _featuredMovieService = featuredMovieService;
    }

    // POST /api/featuredMovie
    [AuthorizeRole(ERoles.Admin)]
    [HttpPost]
    public async Task<ActionResult<FeaturedMovieDto.Read>> AddFeaturedMovie([FromBody] FeaturedMovieDto.Create featuredMovieDto)
    {
        var result = await _featuredMovieService.AddFeaturedMovie(featuredMovieDto);
        if (result == null) return BadRequest("Could not add featured movie.");
        return Ok(result);
    }

    [AuthorizeRole(ERoles.Admin)]
    [HttpPut("{id:int}/position")]
    public async Task<ActionResult<FeaturedMovieDto.Read>> ChangePositionMovie(int id, [FromBody] FeaturedMovieDto.Update featuredMovieDto)
    {
        var result = await _featuredMovieService.ChangePositionMovie(id, featuredMovieDto);
        if (result == null) return NotFound();
        return Ok(result);
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<FeaturedMovieDto.Read>>> ListOfFeaturedMovies()
    {
        var result = await _featuredMovieService.ListOfFeaturedMovies();
        if (result == null || !result.Any()) return NotFound();
        return Ok(result);
    }

    [AuthorizeRole(ERoles.Admin)]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> RemoveFeaturedMovie(int id)
    {
        var result = await _featuredMovieService.RemoveFeaturedMovie(id);
        if (result == null) return NotFound();
        return NoContent();
    }

    [AuthorizeRole(ERoles.Admin)]
    [HttpDelete("removeAll")]
    public async Task<ActionResult> RemoveAllFeaturedMovies()
    {
        var result = await _featuredMovieService.RemoveAllFeaturedMovies();
        if (!result) return BadRequest("Failed to remove all featured movies.");
        return NoContent();
    }
}