using System.ComponentModel.DataAnnotations;

namespace DTO.MovieDTOS;

public class ScreeningDto
{
    public class Create
    {
        [Required(ErrorMessage ="MovieId is required")]
        public int MovieId { get; set; }
        
        [Required(ErrorMessage ="ScreenId is required")]
        public int ScreenId { get; set; }
        
        [Required(ErrorMessage ="StartTime is required")]
        public DateTime StartTime { get; set; }
        
        [Required(ErrorMessage ="EndTime is required")]
        public DateTime EndTime { get; set; }
        
        [Required]
        public decimal BasePrice { get; set; }
        
        [Required]
        public int AvailableSeats { get; set; }
    }
    
    public class Update 
    {
        [Required(ErrorMessage ="MovieId is required")]
        public int MovieId { get; set; }
        
        [Required(ErrorMessage ="ScreenId is required")]
        public int ScreenId { get; set; }
        
        [Required(ErrorMessage ="StartTime is required")]
        public DateTime StartTime { get; set; }
        
        [Required(ErrorMessage ="EndTime is required")]
        public DateTime EndTime { get; set; }
        
        [Required]
        public decimal BasePrice { get; set; }
        
        public bool IsActive { get; set; }
    }

    public class Read
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int ScreenId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal BasePrice { get; set; }
        public int AvailableSeats { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        
        //public virtual Movie Movie { get; set; }
        //public virtual Screen Screen { get; set; }
        //public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        //public virtual ICollection<ScreeningSeatPrice> ScreeningSeatPrices { get; set; } = new List<ScreeningSeatPrice>();
    }
    public class List
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int ScreenId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal BasePrice { get; set; }
        public int AvailableSeats { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ListWithTime
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public decimal BasePrice { get; set; }
    }
}