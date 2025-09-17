using Common.Enums.MovieEnums;
using DAL.EF;
using DTO.MovieDTOS;
using Microsoft.EntityFrameworkCore;
using Repository.DTO;
using Repository.Service;
using MovieEn = DAL.Models.Movie.Movie;

namespace Repository.Movie;

public class MovieRepository : BaseRepository<MovieEn, int>
{
    private readonly IFileStorageService  _fileStorageService;
    public MovieRepository(AppDbContext context, IFileStorageService fileStorageService) :  base(context)
    {
        _fileStorageService = fileStorageService;
    }

    public async Task<MovieEn> CreateMovie(CreateMovie entity)
    {
        if (entity.Poster == null || entity.Trailer == null)
            throw new ArgumentException("Poster and Trailer files are required.");

        string posterUrl = null;
        string trailerUrl = null;
        
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var posterTask = _fileStorageService.SaveFileAsync(entity.Poster.OpenReadStream(), entity.Poster.FileName, "movie");
            var trailerTask = _fileStorageService.SaveFileAsync(entity.Trailer.OpenReadStream(), entity.Trailer.FileName, "movie");

            await Task.WhenAll(posterTask, trailerTask);

            posterUrl = posterTask.Result;
            trailerUrl = trailerTask.Result;
        
            var model = new MovieEn()
            {
                Title = entity.Title,
                Description = entity.Description,
                Genre = entity.Genre,
                Director = entity.Director,
                Duration = entity.Duration,
                Cast = entity.Cast,
                Rating = entity.Rating,
                ReleaseDate = entity.ReleaseDate,
                Language = entity.Language,
                PosterUrl = posterUrl,
                TrailerUrl = trailerUrl,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            
            await _dbSet.AddAsync(model);
            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();
            return model;
        }
        catch
        {
            await _fileStorageService.DeleteFileAsync(posterUrl);
            await _fileStorageService.DeleteFileAsync(trailerUrl);
            throw;
        }
    }
    
    public async Task<IEnumerable<MovieEn>> GetActiveMoviesAsync()
    {
        var movie = await _dbSet.Where(m => m.IsActive == true).ToListAsync();
        
        return movie;
    }

    public async Task<IEnumerable<MovieEn>> GetByGenreAsync(string genre)
    {
        var movie = await _dbSet.Where(m => m.Genre == genre).ToListAsync();
        
        return movie;
    }
    
    public async Task<IEnumerable<CinemaDto.Theater>> FetchMovieWithScreenings(int movieId)
    {
       var movie = await _dbSet
           .Where(m => m.IsActive == true)
           .Include(m => m.Screenings)
           .ThenInclude(sc => sc.Screen)
           .ThenInclude(s => s.Theater)
           .FirstOrDefaultAsync(m => m.Id == movieId);


       var theaterGroup = movie.Screenings
           .GroupBy(s => s.Screen.Theater)
           .Select(theater => new CinemaDto.Theater
           {
               TheaterName = theater.Key.Name,
               Screens = theater
                   .GroupBy(s => s.Screen)
                   .Select(screen => new CinemaDto.Screen
                   {
                       ScreenName = screen.Key.Name,
                       ShowTimes = screen
                           .Select(sc => new CinemaDto.ShowTimes
                           {
                               startTime = sc.StartTime
                           })
                           .OrderBy(st => st.startTime)
                           .ToList()
                   })
                   .ToList()
           })
           .ToList();
       
       
        // TODO should return time span of 7 days counting yesterday
        return theaterGroup;
    }
    
}