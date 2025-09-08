using System.ComponentModel.DataAnnotations;

namespace DTO.MovieDTOS;

public class TheaterDto
{
    public class Create
    {
        [Required(ErrorMessage = "Theater Name is required")]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [StringLength(100, ErrorMessage = "Address must be between 1 and 100 characters")]
        public string? Address { get; set; }
        
        [StringLength(50, ErrorMessage = "Address must be between 1 and 100 characters")]
        public string? City { get; set; }
        
        [Required(ErrorMessage = "Phone is required")]
        [StringLength(20, ErrorMessage = "Address must be between 1 and 100 characters")]
        public string? Phone { get; set; }
    }

    public class Update
    {
        [Required(ErrorMessage = "Theater Name is required")]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [StringLength(100, ErrorMessage = "Address must be between 1 and 100 characters")]
        public string? Address { get; set; }
        
        [StringLength(50, ErrorMessage = "Address must be between 1 and 100 characters")]
        public string? City { get; set; }
        
        [StringLength(20, ErrorMessage = "Address must be between 1 and 100 characters")]
        public string? Phone { get; set; }
        
        public bool IsActive { get; set; }
    }

    public class Read
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        // many screens
    }
    
    public class List
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}