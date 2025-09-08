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
}