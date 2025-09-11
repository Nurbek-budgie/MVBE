using DAL.EF;
using DTO.MovieDTOS;
using Microsoft.EntityFrameworkCore;
using MovieEn = DAL.Models.Movie.Movie;

namespace Repository.Movie;

public class MovieRepository : BaseRepository<DAL.Models.Movie.Movie, int>
{
    public MovieRepository(AppDbContext context) :  base(context)
    {
        
    }

    public async Task<IEnumerable<MovieEn>> GetActiveMoviesAsync()
    {
        var movie = await _dbSet.Where(m => m.IsActive == true).ToListAsync();
        
        return movie;
    }

    public async Task<IEnumerable<DAL.Models.Movie.Movie>> GetByGenreAsync(string genre)
    {
        var movie = await _dbSet.Where(m => m.Genre == genre).ToListAsync();
        
        return movie;
    }

    public async override Task<MovieEn?> GetById(int id)
    {
        // TODO return only future screenings
        return await _dbSet.Include(m => m.Screenings).FirstOrDefaultAsync(m => m.Id == id);
    }
    
    public async Task<IEnumerable<CinemaDto.Theater>> FetchMovieWithScreenings(int movieId)
    {
       var movie = await _dbSet
           .Where(m => m.IsActive == true)
           .Include(m => m.Screenings)
           .ThenInclude(sc => sc.Screen)
           .ThenInclude(s => s.Theater)
           .FirstOrDefaultAsync(m => m.Id == movieId);


       var theaterGroup = movie.Screenings
           .GroupBy(s => s.Screen.Theater)
           .Select(theater => new CinemaDto.Theater
           {
               TheaterName = theater.Key.Name,
               Screens = theater
                   .GroupBy(s => s.Screen)
                   .Select(screen => new CinemaDto.Screen
                   {
                       ScreenName = screen.Key.Name,
                       ShowTimes = screen
                           .Select(sc => new CinemaDto.ShowTimes
                           {
                               startTime = sc.StartTime
                           })
                           .OrderBy(st => st.startTime)
                           .ToList()
                   })
                   .ToList()
           })
           .ToList();
       
       
        // TODO should return time span of 7 days counting yesterday
        return theaterGroup;
    }
}