using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DTO.MovieDTOS;

public class MovieDto
{
    public class Read
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string? Description { get; set; }
            public string? Genre { get; set; }
            public string? Director { get; set; }
            public string? Cast { get; set; }
            public int Duration { get; set; }
            public string? Rating { get; set; }
            public DateTime? ReleaseDate { get; set; }
            public string? PosterUrl { get; set; }
            public string? TrailerUrl { get; set; }
            public string? Language { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
            
            public List<ScreeningDto.ListWithTime> Screenings { get; set; }
        }

       
        public class Create
        {
            [Required(ErrorMessage = "Movie title is required")]
            [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
            public string Title { get; set; } = string.Empty;

            [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
            public string? Description { get; set; }

            [StringLength(100, ErrorMessage = "Genre cannot exceed 100 characters")]
            public string? Genre { get; set; }

            [StringLength(100, ErrorMessage = "Director name cannot exceed 100 characters")]
            public string? Director { get; set; }

            [StringLength(500, ErrorMessage = "Cast information cannot exceed 500 characters")]
            public string? Cast { get; set; }

            [Required(ErrorMessage = "Duration is required")]
            [Range(1, 300, ErrorMessage = "Duration must be between 1 and 300 minutes")]
            public int Duration { get; set; }

            [StringLength(10, ErrorMessage = "Rating cannot exceed 10 characters")]
            public string? Rating { get; set; }

            [DataType(DataType.Date)]
            public DateTime? ReleaseDate { get; set; }

            [StringLength(50, ErrorMessage = "Language cannot exceed 50 characters")]
            public string? Language { get; set; }
            
            public IFormFile? Poster { get; set; }
            
            public IFormFile? Trailer { get; set; }
        }

        
        public class Update
        {
            [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
            public string? Title { get; set; }
            
            [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
            public string? Description { get; set; }
            
            [StringLength(100, ErrorMessage = "Genre cannot exceed 100 characters")]
            public string? Genre { get; set; }
            
            [StringLength(100, ErrorMessage = "Director name cannot exceed 100 characters")]
            public string? Director { get; set; }
            
            [StringLength(500, ErrorMessage = "Cast information cannot exceed 500 characters")]
            public string? Cast { get; set; }
            
            [Range(1, 600, ErrorMessage = "Duration must be between 1 and 600 minutes")]
            public int? Duration { get; set; }
            
            [StringLength(10, ErrorMessage = "Rating cannot exceed 10 characters")]
            public string? Rating { get; set; }
            
            [DataType(DataType.Date)]
            public DateTime? ReleaseDate { get; set; }
            
            [Url(ErrorMessage = "Please provide a valid URL for poster")]
            [StringLength(500, ErrorMessage = "Poster URL cannot exceed 500 characters")]
            public string? PosterUrl { get; set; }
            
            [Url(ErrorMessage = "Please provide a valid URL for trailer")]
            [StringLength(500, ErrorMessage = "Trailer URL cannot exceed 500 characters")]
            public string? TrailerUrl { get; set; }
            
            [StringLength(50, ErrorMessage = "Language cannot exceed 50 characters")]
            public string? Language { get; set; }
            
            public bool? IsActive { get; set; }
        }
        
        public class List
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string? Genre { get; set; }
            public string? Director { get; set; }
            public int Duration { get; set; }
            public string? Rating { get; set; }
            public DateTime? ReleaseDate { get; set; }
            public string? PosterUrl { get; set; }
            public string? TrailerUrl { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreatedAt { get; set; }
        }
}