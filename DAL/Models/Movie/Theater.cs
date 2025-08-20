using System.ComponentModel.DataAnnotations;

namespace DAL.Models.Movie;

public class Theater
{
    public int Id { get; set; }
        
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
        
    [MaxLength(300)]
    public string? Address { get; set; }
        
    [MaxLength(50)]
    public string? City { get; set; }
        
    [MaxLength(20)]
    public string? Phone { get; set; }
        
    public bool IsActive { get; set; } = true;
        
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
    // Navigation properties
    public virtual ICollection<Screen> Screens { get; set; } = new List<Screen>();
}