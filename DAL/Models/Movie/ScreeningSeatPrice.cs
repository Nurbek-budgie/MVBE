using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Movie;

public class ScreeningSeatPrice
{
    public int Id { get; set; }
        
    [Required]
    public int ScreeningId { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(8,2)")]
    public decimal Price { get; set; }
        
    // Navigation properties
    [ForeignKey("ScreeningId")]
    public virtual Screening Screening { get; set; }
}