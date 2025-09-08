using System.ComponentModel.DataAnnotations;
using Common.Enums.MovieEnums;

namespace DTO.Reservation;

public class ReservationDto
{
    public class Create
    {
        [Required]
        [MaxLength(450)] // AspNetUser Id length
        public Guid UserId { get; set; }
        
        [Required(ErrorMessage = "ScreeningId is required")]
        public int ScreeningId { get; set; }
        
        [Required(ErrorMessage = "The amount is required")]
        public decimal TotalAmount { get; set; }
        
        [Required(ErrorMessage = "Payment method is required")]
        public PaymentMethod PaymentMethod { get; set; }
        
        //public DateTime? ExpiresAt { get; set; } // For temporary holds
        
        // Navigation properties
        //[ForeignKey("ScreeningId")]
        //public virtual Screening Screening { get; set; }
        
        //[ForeignKey("UserId")]
        //public virtual User User { get; set; }
        
        //public virtual ICollection<ReservedSeat> ReservedSeats { get; set; } = new List<ReservedSeat>();
    }
    
    public class Update
    {
        public BookingStatus BookingStatus { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string? TransactionId { get; set; }
    }
    public class Read
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int ScreeningId { get; set; }
        public string ReservationNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string? TransactionId { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Nested DTOs for richer responses
        //public ScreeningDto.List? Screening { get; set; }
        //public List<ReservedSeatDto.Read> ReservedSeats { get; set; } = new();
    }
}