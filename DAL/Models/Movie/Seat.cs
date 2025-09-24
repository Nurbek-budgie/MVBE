using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Movie;

public class Seat
{
    public int Id { get; set; }
        
    [Required]
    public int ScreenId { get; set; }
        
    [Required]
    [MaxLength(10)]
    public string SeatNumber { get; set; } // 1, 2, 3,4 ,5, 6 ,7
        
    [Required]
    [MaxLength(5)]
    public string RowNumber { get; set; } // A, B, C, D
        
    public bool IsActive { get; set; } = true;
        
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
    // Navigation properties
    [ForeignKey("ScreenId")]
    public virtual Screen Screen { get; set; }
        
    public virtual ICollection<ReservedSeat> ReservedSeats { get; set; } = new List<ReservedSeat>();
}