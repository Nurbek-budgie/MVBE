using AutoMapper;
using BLL.Interfaces.Reservation;
using DTO.Reservation;
using Repository.Reservation;

namespace BLL.Services.Reservation;

public class ReservationService : IReservationService
{
    private readonly ReservationRepository _reservationRepository;
    private readonly IMapper _mapper;
    
    public ReservationService(ReservationRepository  reservationRepository, IMapper mapper)
    {
        _reservationRepository = reservationRepository;
        _mapper = mapper;
    }
    
    public async Task<ReservationDto.Read> CreateReservation(Guid userId, ReservationDto.ReserveSeat reservation)
    {
        // Nurbek(AspnetUser), seats(a1,a2)(Seats), movieId(Movie), screenId(Screen)
        if (reservation == null)
        {
            throw new ArgumentNullException(nameof(reservation));
        }
        var seats = reservation.Seats;

        var tupleSeats = new List<Tuple<string, string>>();
        foreach (var seat in seats)
        {
            tupleSeats.Add(Tuple.Create(seat.Row, seat.Number));
        }
        
        var reservedEntity = await _reservationRepository.ReserveAsync(userId, tupleSeats, reservation.ScreeningId);

        if (reservedEntity == null)
        {
            throw new ArgumentNullException(nameof(reservedEntity));
        }
        
        return _mapper.Map<ReservationDto.Read>(reservedEntity);
       }

    public async Task<ReservationDto.Read> UpdateReservation(int reserveId, ReservationDto.Update reservation)
    {
        if (await CheckExist(reserveId))
        {
            throw new ArgumentException($"Reservation with id {reserveId} does not exist");
        }
        
        if (reservation == null)
        {
            throw new ArgumentNullException(nameof(reservation));
        }

        var entity = await _reservationRepository.UpdateReservationStatus(reserveId, reservation.BookingStatus,
            reservation.PaymentStatus, reservation.PaymentMethod);
        
        return _mapper.Map<ReservationDto.Read>(entity);
    }

    public async Task<bool> CheckExist(int reserveId)
    {
        var check = await _reservationRepository.Exists(reserveId);
        return !check;
    }
}