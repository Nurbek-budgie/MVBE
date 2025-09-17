using Common.Enums.MovieEnums;
using DAL.EF;
using DAL.Models.Movie;
using DTO.MovieDTOS;
using Microsoft.EntityFrameworkCore;

namespace Repository.Movie;

public class ScreenRepository : BaseRepository<Screen, int>
{
    public ScreenRepository(AppDbContext context) :base(context)
    {
        
    }

    public async override Task<Screen> GetById(int id)
    {
        var screen = _dbSet
            .Include(s => s.Screenings)
            .Include(s => s.Theater)
            .Include(s => s.Seats)
            .FirstOrDefault(s => s.Id == id);
        
        return screen;
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
        
        screen.TotalSeats = counter;
        screen.Seats = seats;
        
        await _dbContext.SaveChangesAsync();
        
        return screen;
    }


    public async Task<IEnumerable<ScreenTypeDto.Cinema>> GetScreenTypes(ScreenType screenType)
    {
        var screen = await _dbSet
            .Include(s => s.Theater)
            .Include(s => s.Screenings)
            .ThenInclude(s => s.Movie)
            .Where(x => x.ScreenType == screenType)
            .ToListAsync();

        if (screen == null)
        {
            return new List<ScreenTypeDto.Cinema>();
        }
        
        var screenTypes = screen.GroupBy(x => x.Theater)
            .Select(theater => new ScreenTypeDto.Cinema
            {
                CinemaId = theater.Key.Id,
                CinemaName = theater.Key.Name,
                Movies = theater.SelectMany(s => s.Screenings).GroupBy(sc => sc.Movie)
                    .Select(movies => new ScreenTypeDto.Movie
                    {
                        MovieId = movies.Key.Id,
                        MovieName = movies.Key.Title,
                        Showtimes = movies.Select(showtimes => new ScreenTypeDto.Showtime
                        {
                            Time = showtimes.StartTime
                        }).ToList()
                    }).ToList()
            }).ToList();
        
        return screenTypes;
    }
}