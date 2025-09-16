using DTO.MovieDTOS;

namespace BLL.Interfaces.Movie;

public interface IFeaturedMovieService
{
    Task<FeaturedMovieDto.Read> AddFeaturedMovie(FeaturedMovieDto.Create featuredMovieDto);
    Task<FeaturedMovieDto.Read> ChangePositionMovie(int id, FeaturedMovieDto.Update featuredMovieDto);
    Task<IEnumerable<FeaturedMovieDto.Read>> ListOfFeaturedMovies();
    Task<bool> RemoveFeaturedMovie(int id);
    Task<bool> RemoveAllFeaturedMovies();
}