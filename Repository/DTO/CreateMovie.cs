using Microsoft.AspNetCore.Http;

namespace Repository.DTO;

public class CreateMovie
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? Genre { get; set; }
    public string? Director { get; set; }
    public string? Cast { get; set; }
    public int Duration { get; set; }
    public string? Rating { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public string? Language { get; set; }
    public IFormFile? Poster { get; set; }
    public IFormFile? Trailer { get; set; }
}