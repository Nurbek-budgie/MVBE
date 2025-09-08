using DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace Repository.Movie;

public class MovieRepository : BaseRepository<DAL.Models.Movie.Movie, int>
{
    public MovieRepository(AppDbContext context) :  base(context)
    {
        
    }

    public async Task<IEnumerable<DAL.Models.Movie.Movie>> GetActiveMoviesAsync()
    {
        var movie = await _dbSet.Where(m => m.IsActive == true).ToListAsync();
        
        return movie;
    }

    public async Task<IEnumerable<DAL.Models.Movie.Movie>> GetByGenreAsync(string genre)
    {
        var movie = await _dbSet.Where(m => m.Genre == genre).ToListAsync();
        
        return movie;
    }
}