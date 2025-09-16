using System.ComponentModel.DataAnnotations;

namespace DAL.Models.Movie;

public class FeaturedMovie
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int MovieId { get; set; }
    
    public int? Position { get; set; }
    
    public virtual Movie Movie { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}