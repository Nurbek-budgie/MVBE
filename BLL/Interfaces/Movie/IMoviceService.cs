namespace BLL.Interfaces.Movie;

public interface IMovieService
{
    Task<IEnumerable<MovieDto>> GetAllMoviesAsync();
    Task<IEnumerable<MovieDto>> GetActiveMoviesAsync();
    Task<MovieDto?> GetMovieByIdAsync(int id);
    Task<IEnumerable<MovieDto>> GetMoviesByGenreAsync(string genre);
    Task<MovieDto> CreateMovieAsync(CreateMovieDto createMovieDto);
    Task<MovieDto> UpdateMovieAsync(int id, UpdateMovieDto updateMovieDto);
    Task<bool> DeleteMovieAsync(int id);
    Task<bool> MovieExistsAsync(int id);
}