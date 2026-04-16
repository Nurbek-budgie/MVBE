using System.Data;
using Common.Enums.MovieEnums;
using DAL.EF;
using DAL.Models.Movie;
using Microsoft.EntityFrameworkCore;
using Npgsql;
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

    var screening = await _dbContext.Screenings
        .Include(s => s.Screen)
        .ThenInclude(sc => sc.Seats)
        .FirstOrDefaultAsync(s => s.Id == screeningId);

    if (screening == null)
        throw new KeyNotFoundException("Screening not found");

    var screen = screening.Screen;
    if (screen == null)
        throw new InvalidOperationException("Screen not found");

    var reservedSeats = new List<ReservedSeat>();
    foreach (var (row, number) in seatRequests)
    {
        var seat = screen.Seats.FirstOrDefault(s => s.RowNumber == row && s.SeatNumber == number);
        if (seat == null)
            throw new InvalidOperationException($"Seat {row}{number} does not exist in this screen");

        reservedSeats.Add(new ReservedSeat
        {
            SeatId = seat.Id,
            ScreeningId = screeningId
        });
    }

    var reservation = new ReservationEntity
    {
        UserId = userId,
        ScreeningId = screeningId,
        ReservationNumber = $"RES{DateTime.UtcNow:yyyyMMddHHmmssfff}",
        TotalAmount = reservedSeats.Count * screening.BasePrice,
        BookingStatus = BookingStatus.Confirmed,
        PaymentStatus = PaymentStatus.Pending,
        PaymentMethod = PaymentMethod.Cash,
        BookingDate = DateTime.UtcNow,
        ReservedSeats = reservedSeats
    };

    await using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);
    try
    {
        _dbContext.Reservations.Add(reservation);
        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();
        return reservation;
    }
    catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
    {
        await transaction.RollbackAsync();
        throw new InvalidOperationException("One or more selected seats are already taken for this screening.");
    }
}


public async Task<ReservationEntity> UpdateReservationStatus(int reserveId, BookingStatus status, PaymentStatus paymentStatus, PaymentMethod paymentMethod)
{
    var entity = await _dbContext.Reservations
        .FirstOrDefaultAsync(rs => rs.Id == reserveId);
    if (entity == null)
        throw new KeyNotFoundException("Reservation not found");

    entity.BookingStatus = status;
    entity.PaymentStatus = paymentStatus;
    entity.PaymentMethod = paymentMethod;

    _dbContext.Reservations.Update(entity);
    await _dbContext.SaveChangesAsync();

    return entity;
}
}
