using DAL.EF;
using DAL.Models.Movie;
using Microsoft.EntityFrameworkCore;

namespace Repository.Movie;

public class ScreenRepository : BaseRepository<Screen, int>
{
    public ScreenRepository(AppDbContext context) :base(context)
    {
        
    }

    public async Task<IEnumerable<Screen>> GetAllActiveScreens()
    {
        var screens = _dbSet.Where(s => s.IsActive == true).ToListAsync();
        return await screens;
    }
}