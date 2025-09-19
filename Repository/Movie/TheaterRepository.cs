using DAL.EF;
using DAL.Models.Movie;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository.Service;

namespace Repository.Movie;

public class TheaterRepository : BaseRepository<Theater, int>
{
    private readonly IFileStorageService  _fileStorageService;
    public TheaterRepository(AppDbContext  dbContext, IFileStorageService fileStorageService) : base(dbContext)
    {
        _fileStorageService = fileStorageService;
    }

    public async Task<Theater> UploadLogoTheater(int theaterId, IFormFile logo)
    {
        string logoUrl = null;
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var entity = await _dbSet.FirstOrDefaultAsync(x => x.Id == theaterId);
            var url = await _fileStorageService.SaveFileAsync(logo.OpenReadStream(), logo.FileName, "Logo");
            
            logoUrl = url;
            entity.LogoUrl = logoUrl;
            
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
            transaction.Commit();
            
            return entity;
        }
        catch
        {
            await _fileStorageService.DeleteFileAsync(logoUrl);
            throw;
        }
    }
    
    public async Task<IEnumerable<Theater>> GetActiveTheaterAsync()
    {
        return await _dbSet.Where(t => t.IsActive == true).ToListAsync();
    }

    public async Task<IEnumerable<Theater>> FetchMovieWithScreenings()
    {
        var movie = await _dbSet
            .Include(t => t.Screens)
            .ThenInclude(s => s.Screenings)
            .ThenInclude(s => s.Movie)
            .ToListAsync();
            
        // TODO should return time span of 7 days counting yesterday
        return movie;
    }
}