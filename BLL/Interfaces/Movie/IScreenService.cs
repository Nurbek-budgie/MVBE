using DTO.MovieDTOS;

namespace BLL.Interfaces.Movie;

public interface IScreenService
{
    Task<ScreenDto.Read> CreateScreenAsync(ScreenDto.Create screen);
    Task<ScreenDto.Read?> UpdateScreenAsync(int id,ScreenDto.Update screen);
    Task<ScreenDto.Read> GetScreenByIdAsync(int id);
    Task<IEnumerable<ScreenDto.List>> GetAllScreens();
    Task<IEnumerable<ScreenDto.List>> GetAllActiveScreens();
    Task<bool> DeleteScreenAsync(int id);
}