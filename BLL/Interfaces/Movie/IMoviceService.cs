using DTO.MovieDTOS;

namespace BLL.Interfaces.Movie;

public interface IMovieService
{
    Task<IEnumerable<MovieDto.List>> GetAllMoviesAsync();
    Task<IEnumerable<MovieDto.List>> GetActiveMoviesAsync();
    Task<MovieDto.Read?> GetMovieByIdAsync(int id);
    Task<IEnumerable<CinemaDto.Theater>> GetMovieGroupedByTheater(int movieId);
    Task<MovieDto.ReadWithScreenings> GetMovieByIdWithScreenings(int id);
    Task<MovieDto.Read> CreateMovieAsync(MovieDto.Create createMovieDto);
    Task<MovieDto.Read> UpdateMovieAsync(int id, MovieDto.Update updateMovieDto);
    Task<bool> DeleteMovieAsync(int id);
    Task<bool> MovieExistsAsync(int id);
}