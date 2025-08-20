using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Movie;

public class Screening
{
    public int Id { get; set; }
        
    [Required]
    public int MovieId { get; set; }
        
    [Required]
    public int ScreenId { get; set; }
        
    [Required]
    public DateTime StartTime { get; set; }
        
    [Required]
    public DateTime EndTime { get; set; }
        
    [Required]
    [Column(TypeName = "decimal(8,2)")]
    public decimal BasePrice { get; set; }
        
    [Required]
    public int AvailableSeats { get; set; }
        
    public bool IsActive { get; set; } = true;
        
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
    // Navigation properties
    [ForeignKey("MovieId")]
    public virtual Movie Movie { get; set; }
        
    [ForeignKey("ScreenId")]
    public virtual Screen Screen { get; set; }
        
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    public virtual ICollection<ScreeningSeatPrice> ScreeningSeatPrices { get; set; } = new List<ScreeningSeatPrice>();
}
