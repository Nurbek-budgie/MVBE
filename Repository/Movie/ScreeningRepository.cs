using DAL.EF;
using DAL.Models.Movie;
using Microsoft.EntityFrameworkCore;

namespace Repository.Movie;

public class ScreeningRepository : BaseRepository<Screening, int>
{
    public ScreeningRepository(AppDbContext context) : base(context)
    {
        
    }

    public async Task<IEnumerable<Screening>> GetActiveScreeningAsync()
    {
        var screening = await _dbSet.Where(screening => screening.IsActive == true).ToListAsync();
        return screening;
    }
}