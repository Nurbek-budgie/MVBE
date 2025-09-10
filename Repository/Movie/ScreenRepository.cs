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

    public override async Task<Screen> Create(Screen entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        
        var createdEntity = await _dbSet.AddAsync(entity);
        return createdEntity.Entity;
    }

    public async Task<Screen> PopulateSeats(Screen screen, int totalRows, int seatsPerRow)
    {
        var seats = new List<Seat>();
        int counter = 0;
        for (var i = 1; i <= totalRows; i++) // Row index
        {
            string rowLetter = ((char)('A' + (i - 1))).ToString(); // 1 -> A, 2 -> B, etc.

            for (var j = 1; j <= seatsPerRow; j++) // Seat index
            {
                var newSeat = new Seat
                {
                    ScreenId = screen.Id,
                    RowNumber = rowLetter,
                    SeatNumber = j.ToString()
                };
                counter++;
                seats.Add(newSeat);
            }
        }

        //_dbContext.Seats.AddRangeAsync(seats);
        screen.TotalSeats = counter;
        screen.Seats = seats;
        
        await _dbContext.SaveChangesAsync();
        
        return screen;
    }
}