namespace DTO.MovieDTOS;

public class CinemaDto
{
    public class Theater
    {
        public string TheaterName { get; set; }
        
        public List<Screen> Screens { get; set; }
    }

    public class Screen
    {
        public string ScreenName { get; set; }
        public List<ShowTimes>  ShowTimes { get; set; }
    }

    public class ShowTimes
    {
        public DateTime startTime { get; set; }
    }
}