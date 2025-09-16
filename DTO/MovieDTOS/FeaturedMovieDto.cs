using System.ComponentModel.DataAnnotations;

namespace DTO.MovieDTOS;

public class FeaturedMovieDto
{
    public class Read
    {
        public int Id { get; set; }
        public string PosterUrl { get; set; }
        public int Position { get; set; }
    }

    public class Create
    {
        [Required]
        public int MovieId { get; set; }
        public int Position { get; set; }
    }

    public class Update
    {
        [Required]
        public int Position { get; set; }
    }
}