namespace DTO.Reservation;

public class ScreenSeatDto
{
    public class Result
    {
        public int ScreeningId { get; set; }
        public List<string> Row { get; set; }
        public List<string> SeatNumber { get; set; }
        public List<Seat> Seats { get; set; }
    }

    public class Seat
    {
        public int SeatId { get; set; }
        public string Row { get; set; }
        public string SeatNumber { get; set; }
        public string SeatName { get; set; }
        public bool IsBooked { get; set; }
    }
}