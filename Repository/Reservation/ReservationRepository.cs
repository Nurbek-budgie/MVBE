using Common.Enums.MovieEnums;
using DAL.EF;
using DAL.Models.Movie;
using Microsoft.EntityFrameworkCore;
using ReservationEntity = DAL.Models.Movie.Reservation;


namespace Repository.Reservation;

public class ReservationRepository : BaseRepository<ReservationEntity, int>
{

    public ReservationRepository(AppDbContext context) : base(context)
    {
        
    }

public async Task<ReservationEntity> ReserveAsync(Guid userId, List<Tuple<string, string>> seatRequests, int screeningId)
{
    if (seatRequests == null || !seatRequests.Any())
        throw new ArgumentException("At least one seat must be selected", nameof(seatRequests));

    // Step 1: validate screening exists
    var screening = await _dbContext.Screenings
        .Include(s => s.Screen)
        .ThenInclude(sc => sc.Seats)
        .FirstOrDefaultAsync(s => s.Id == screeningId);

    if (screening == null)
        throw new KeyNotFoundException("Screening not found");

    // Step 2: check requested seats
    var reservedSeats = new List<ReservedSeat>();

    foreach (var (row, number) in seatRequests)
    {
        var seat = screening.Screen.Seats
            .FirstOrDefault(s => s.RowNumber == row && s.SeatNumber == number);

        if (seat == null)
            throw new InvalidOperationException($"Seat {row}{number} does not exist in this screen");

        // Check if already reserved for this screening
        bool isTaken = await _dbContext.ReservedSeats
            .AnyAsync(rs => rs.SeatId == seat.Id && rs.Reservation.ScreeningId == screeningId);

        if (isTaken)
            throw new InvalidOperationException($"Seat {row}{number} is already reserved");

        reservedSeats.Add(new ReservedSeat
        {
            SeatId = seat.Id,
            // TODO
        });
    }

    // Step 3: create reservation
    var reservation = new ReservationEntity
    {
        UserId = userId,
        ScreeningId = screeningId,
        ReservationNumber = $"RES{DateTime.UtcNow:yyyyMMddHHmmssfff}", // simple generator
        TotalAmount = reservedSeats.Count * screening.BasePrice, // assuming you have pricing
        BookingStatus = BookingStatus.Confirmed,
        PaymentStatus = PaymentStatus.Pending,
        PaymentMethod = PaymentMethod.Cash,
        BookingDate = DateTime.UtcNow,
        ReservedSeats = reservedSeats
    };

    // Step 4: save transactionally
    _dbContext.Reservations.Add(reservation);
    await _dbContext.SaveChangesAsync();

    return reservation;
}

}