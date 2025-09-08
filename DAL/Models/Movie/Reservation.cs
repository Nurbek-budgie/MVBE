using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Enums.MovieEnums;

namespace DAL.Models.Movie;

public class Reservation
{
    public int Id { get; set; }
        
    [Required]
    [MaxLength(450)] // AspNetUser Id length
    public Guid UserId { get; set; }
        
    [Required]
    public int ScreeningId { get; set; }
        
    [Required]
    [MaxLength(20)]
    public string ReservationNumber { get; set; } // e.g., RES20240820001
        
    [Required]
    [Column(TypeName = "decimal(8,2)")]
    public decimal TotalAmount { get; set; }
        
    [MaxLength(20)]
    public BookingStatus BookingStatus { get; set; } =  BookingStatus.Confirmed;
        
    [MaxLength(20)]
    public PaymentMethod PaymentMethod { get; set; } =  PaymentMethod.Cash;

    [MaxLength(50)]
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Completed;
        
    [MaxLength(100)]
    public string? TransactionId { get; set; } // Payment gateway transaction ID
        
    public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        
    public DateTime? ExpiresAt { get; set; } // For temporary holds
        
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
    // Navigation properties
    [ForeignKey("ScreeningId")]
    public virtual Screening Screening { get; set; }
        
    [ForeignKey("UserId")]
    public virtual User User { get; set; }
        
    public virtual ICollection<ReservedSeat> ReservedSeats { get; set; } = new List<ReservedSeat>();
}