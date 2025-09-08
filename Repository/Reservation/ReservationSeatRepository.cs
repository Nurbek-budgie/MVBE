using DAL.EF;
using DAL.Models.Movie;

namespace Repository.Reservation;

public class ReservationSeatRepository : BaseRepository<ReservedSeat, int>
{

    public ReservationSeatRepository(AppDbContext context) : base(context)
    {
        
    }
    
    
}