using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Movie;

public class ReservedSeat
{
    public int Id { get; set; }
        
    [Required]
    public int ReservationId { get; set; }
        
    [Required]
    public int SeatId { get; set; }
        
    [Required]
    [Column(TypeName = "decimal(8,2)")]
    public decimal Price { get; set; }
        
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
    // Navigation properties
    [ForeignKey("ReservationId")]
    public virtual Reservation Reservation { get; set; }
        
    [ForeignKey("SeatId")]
    public virtual Seat Seat { get; set; }
}