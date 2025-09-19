using DTO.MovieDTOS;
using Microsoft.AspNetCore.Http;

namespace BLL.Interfaces.Movie;

public interface ITheaterService
{
    Task<IEnumerable<TheaterDto.List>> GetAllTheatersAsync();
    Task<IEnumerable<TheaterDto.List>> GetAllActiveTheatersAsync();
    Task<TheaterDto.Read> CreateAsync(TheaterDto.Create theater);
    Task<TheaterDto.Read> UploadLogoImage(int id, IFormFile logo);
    Task<TheaterDto.Read?> UpdateAsync(int id, TheaterDto.Update updatedtheater);
    Task<TheaterDto.Read?> GetTheaterAsync(int id);
    Task<bool> DeleteTheater(int id);
}