using DAL.EF;

namespace Repository.Movie;

public class MovieRepository : BaseRepository<DAL.Models.Movie.Movie, int>
{
    public MovieRepository(AppDbContext context) :  base(context)
    {
        
    }
    
}