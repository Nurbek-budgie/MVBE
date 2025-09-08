using System.ComponentModel.DataAnnotations;
using Common.Enums.MovieEnums;

namespace DTO.MovieDTOS;

public class ScreenDto
{
    public class Create
    {
        [Required(ErrorMessage ="TheaterId is required")]
        public int TheaterId { get; set; }
        
        [Required(ErrorMessage ="Screen Name is required")]
        [MaxLength(50)]
        public string Name { get; set; } // Screen 1, Hall A, etc.
        
        [Required(ErrorMessage ="TotalSeats is required")]
        public int TotalSeats { get; set; }
        
        public ScreenType ScreenType { get; set; }
    }

    public class Update
    {
        [Required(ErrorMessage ="TheaterId is required")]
        public int TheaterId { get; set; }
        
        [Required(ErrorMessage ="Screen Name is required")]
        [MaxLength(50)]
        public string Name { get; set; } // Screen 1, Hall A, etc.
        
        [Required(ErrorMessage ="TotalSeats is required")]
        public int TotalSeats { get; set; }
        
        public ScreenType ScreenType { get; set; }
        
        public bool IsActive { get; set; }
    }

    public class Read
    {
        public int Id { get; set; }
        public int TheaterId { get; set; }
        public string Name { get; set; } // Screen 1, Hall A, etc.
        public int TotalSeats { get; set; }
        public ScreenType ScreenType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    
    public class List
    {
        public int Id { get; set; }
        public int TheaterId { get; set; }
        public string Name { get; set; } // Screen 1, Hall A, etc.
        public int TotalSeats { get; set; }
        public ScreenType ScreenType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}