using DTO.MovieDTOS;

namespace BLL.Interfaces.Movie;

public interface IScreeningService
{
    Task<ScreeningDto.Read> CreateScreeningAsync(ScreeningDto.Create screeningDto);
    Task<ScreeningDto.Read?> UpdateScreeningAsync(int id, ScreeningDto.Update screeningDto);
    Task<ScreeningDto.Read> GetScreeningIdAsync(int id);
    Task<IEnumerable<ScreeningDto.List>> GetAllActiveScreeningAsync();
    Task<IEnumerable<ScreeningDto.List>> GetAllScreeningAsync();
    Task<bool> DeleteScreeningAsync(int id);
    
}