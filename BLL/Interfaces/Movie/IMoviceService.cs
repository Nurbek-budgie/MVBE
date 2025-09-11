using DTO.MovieDTOS;

namespace BLL.Interfaces.Movie;

public interface IMovieService
{
    Task<IEnumerable<MovieDto.List>> GetAllMoviesAsync();
    Task<IEnumerable<MovieDto.List>> GetActiveMoviesAsync();
    Task<MovieDto.Read?> GetMovieByIdAsync(int id);
    Task<IEnumerable<MovieDto.List>> GetMoviesByGenreAsync(string genre);
    Task<IEnumerable<CinemaDto.Theater>> Getmov(int movieId);
    Task<MovieDto.Read> CreateMovieAsync(MovieDto.Create createMovieDto);
    Task<MovieDto.Read> UpdateMovieAsync(int id, MovieDto.Update updateMovieDto);
    Task<bool> DeleteMovieAsync(int id);
    Task<bool> MovieExistsAsync(int id);
}