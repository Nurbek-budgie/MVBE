using DAL.EF;
using DAL.Models.Movie;

namespace Repository.Reservation;

public class SeatRepository : BaseRepository<Seat, int>
{

    public SeatRepository(AppDbContext context) : base(context)
    {
        
    }
}