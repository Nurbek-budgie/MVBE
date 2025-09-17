namespace DTO.MovieDTOS;

public class ScreenTypeDto
{
    public class Cinema
    {
        public int CinemaId { get; set; }
        public string CinemaName { get; set; }
        public List<Movie> Movies { get; set; }
    }

    public class Movie
    {
        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public List<Showtime> Showtimes { get; set; }
    }

    public class Showtime
    {
        public DateTime Time { get; set; }
    }
}