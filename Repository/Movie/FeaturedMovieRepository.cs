using DAL.EF;
using DAL.Models.Movie;
using Microsoft.EntityFrameworkCore;

namespace Repository.Movie;

public class FeaturedMovieRepository : BaseRepository<FeaturedMovie, int>
{
    public FeaturedMovieRepository(AppDbContext context) : base(context)
    {
        
    }

    public async override Task<FeaturedMovie> Create(FeaturedMovie entity)
    {
        await _dbContext.FeaturedMovies.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        var featuredMovie = await _dbContext.FeaturedMovies.Include(x => x.Movie).FirstOrDefaultAsync(m => m.Id == entity.Id);
        
        return featuredMovie;
    }
    public async override Task<FeaturedMovie> GetById(int id)
    {
        var featuredMovie = _dbSet.Include(x => x.Movie).FirstOrDefaultAsync(m => m.Id == id);
        return await featuredMovie;
    }


    public async override Task<IEnumerable<FeaturedMovie>> GetAll()
    {
        var movies = await _dbSet.Include(x => x.Movie).OrderBy(x => x.Position).ToListAsync();
        return movies;
    }
    public async Task<bool> DeleteAllAsync()
    {
        var allFeatured = _dbContext.FeaturedMovies.ToList();
        _dbContext.FeaturedMovies.RemoveRange(allFeatured);
        return await _dbContext.SaveChangesAsync() > 0;
    }
}