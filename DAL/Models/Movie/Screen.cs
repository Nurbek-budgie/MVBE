using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Enums.MovieEnums;

namespace DAL.Models.Movie;

public class Screen
{
    public int Id { get; set; }
        
    [Required]
    public int TheaterId { get; set; }
        
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } // Screen 1, Hall A, etc.
        
    [Required]
    public int TotalSeats { get; set; }
        
    [MaxLength(50)]
    public ScreenType  ScreenType { get; set; }
        
    public bool IsActive { get; set; } = true;
        
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
    // Navigation properties
    [ForeignKey("TheaterId")]
    public virtual Theater Theater { get; set; }
        
    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
    public virtual ICollection<Screening> Screenings { get; set; } = new List<Screening>();
}