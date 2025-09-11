using DAL.EF;
using DAL.Models.Movie;
using Microsoft.EntityFrameworkCore;

namespace Repository.Movie;

public class TheaterRepository : BaseRepository<Theater, int>
{
    public TheaterRepository(AppDbContext  dbContext) : base(dbContext)
    {
        
    }

    public async Task<IEnumerable<Theater>> GetActiveTheaterAsync()
    {
        return await _dbSet.Where(t => t.IsActive == true).ToListAsync();
    }

    public async Task<IEnumerable<Theater>> FetchMovieWithScreenings()
    {
        var movie = await _dbSet
            .Include(t => t.Screens)
            .ThenInclude(s => s.Screenings)
            .ThenInclude(s => s.Movie)
            .ToListAsync();
            
        // TODO should return time span of 7 days counting yesterday
        return movie;
    }
}