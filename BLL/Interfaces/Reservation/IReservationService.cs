using DTO.Reservation;

namespace BLL.Interfaces.Reservation;

public interface IReservationService
{
    public Task<ReservationDto.Read> CreateReservation(Guid userId, ReservationDto.ReserveSeat reservation);
    public Task<ReservationDto.Read> UpdateReservation(int reserveId, ReservationDto.Update reservation);
}