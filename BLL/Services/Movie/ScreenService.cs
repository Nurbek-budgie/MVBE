using AutoMapper;
using BLL.Interfaces.Movie;
using DAL.Models.Movie;
using DTO.MovieDTOS;
using Microsoft.VisualBasic;
using Repository.Movie;

namespace BLL.Services.Movie;

public class ScreenService : IScreenService
{
    private readonly ScreenRepository _screenRepository;
    private readonly IMapper _mapper;

    public ScreenService(ScreenRepository screenRepository, IMapper mapper)
    {
        _screenRepository = screenRepository;
        _mapper = mapper;
    }

    public async Task<ScreenDto.Read> CreateScreenAsync(ScreenDto.Create screen)
    {
        if (screen == null)
            throw new ArgumentNullException(nameof(screen));
    
        var entity = _mapper.Map<Screen>(screen);
        entity.IsActive = true;
    
        
        var result = await _screenRepository.Create(entity);
        result = await _screenRepository.PopulateSeats(entity, screen.Row, screen.Number);
        
        return _mapper.Map<ScreenDto.Read>(result);
    }

    public async Task<ScreenDto.Read?> UpdateScreenAsync(int id, ScreenDto.Update screen)
    {
        if (screen == null)
            throw new ArgumentNullException(nameof(screen));
        
        if (await CheckExists(id))
        {
            throw new KeyNotFoundException();
        }
        
        var entity = await _screenRepository.GetById(id);
        
        _mapper.Map(screen, entity);

        var result = await _screenRepository.Update(entity);
        
        return _mapper.Map<ScreenDto.Read>(result);
    }

    public async Task<ScreenDto.Read> GetScreenByIdAsync(int id)
    {
        if (await CheckExists(id))
        {
            throw new KeyNotFoundException();
        }
        var entity = await _screenRepository.GetById(id);
        
        return _mapper.Map<ScreenDto.Read>(entity);
    }

    public async Task<IEnumerable<ScreenDto.List>> GetAllScreens()
    {
        var screens = await _screenRepository.GetAll();
        return _mapper.Map<IEnumerable<ScreenDto.List>>(screens);
    }

    public async Task<IEnumerable<ScreenDto.List>> GetAllActiveScreens()
    {
        var screens = await _screenRepository.GetAllActiveScreens();
        return _mapper.Map<IEnumerable<ScreenDto.List>>(screens);
    }

    public async Task<bool> DeleteScreenAsync(int id)
    {
        if (await CheckExists(id))
        {
            throw new KeyNotFoundException();
        }
        
        var entity = await _screenRepository.GetById(id);
        if (entity == null)
        {
            return false;
        }
        
        entity.IsActive = false;
        await _screenRepository.Update(entity);
        
        return true;
    }

    public async Task<bool> CheckExists(int id)
    {
        var check = await _screenRepository.Exists(id);
        return !check;
    }
}