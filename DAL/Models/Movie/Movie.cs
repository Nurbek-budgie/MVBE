using System.ComponentModel.DataAnnotations;

namespace DAL.Models.Movie;

public class Movie
{
    public int Id { get; set; }
        
    [Required]
    [MaxLength(200)]
    public string Title { get; set; }
        
    [MaxLength(1000)]
    public string? Description { get; set; }
        
    [MaxLength(100)]
    public string? Genre { get; set; }
        
    [MaxLength(100)]
    public string? Director { get; set; }
        
    [MaxLength(500)]
    public string? Cast { get; set; }
        
    [Required]
    public int Duration { get; set; } // in minutes
        
    [MaxLength(10)]
    public string? Rating { get; set; } // PG, PG-13, R, etc.
        
    public DateTime? ReleaseDate { get; set; }
        
    [MaxLength(500)]
    public string? PosterUrl { get; set; }
        
    [MaxLength(500)]
    public string? TrailerUrl { get; set; }
        
    [MaxLength(50)]
    public string? Language { get; set; }
        
    public bool IsActive { get; set; } = true;
        
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
    // Navigation properties
    public virtual ICollection<Screening> Screenings { get; set; } = new List<Screening>();
}