using AutoMapper;
using BLL.Interfaces.Movie;
using DAL.Models.Movie;
using DTO.MovieDTOS;
using Repository.Movie;

namespace BLL.Services.Movie;

public class TheaterService : ITheaterService
{
    private readonly TheaterRepository _repository;
    private readonly IMapper _mapper;

    public TheaterService(TheaterRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<TheaterDto.List>> GetAllTheatersAsync()
    {
        var theaters = await _repository.GetAll();
        return _mapper.Map<IEnumerable<TheaterDto.List>>(theaters);
    }

    public async Task<IEnumerable<TheaterDto.List>> GetAllActiveTheatersAsync()
    {
        var theaters = await _repository.GetActiveTheaterAsync();
        return _mapper.Map<IEnumerable<TheaterDto.List>>(theaters);
    }

    public async Task<TheaterDto.Read> CreateAsync(TheaterDto.Create theater)
    {
        if (theater == null) 
            throw new ArgumentNullException(nameof(theater));
        
        var entity = _mapper.Map<Theater>(theater);
        
        entity.IsActive = true;
        
        var result = await _repository.Create(entity);
        
        return _mapper.Map<TheaterDto.Read>(result);
    }

    public async Task<TheaterDto.Read?> UpdateAsync(int id, TheaterDto.Update updatedtheater)
    {
        if (updatedtheater == null)
            throw new ArgumentNullException(nameof(updatedtheater));
        
        if (await CheckExists(id))
        {
            throw new KeyNotFoundException();
        }
        
        var entity = await _repository.GetById(id);
        _mapper.Map(updatedtheater, entity);
        var result = await _repository.Update(entity);
        
        return _mapper.Map<TheaterDto.Read>(result);
    }

    public async Task<TheaterDto.Read> GetTheaterAsync(int id)
    {
        if (await CheckExists(id))
        {
            throw new KeyNotFoundException();
        }
        
        return _mapper.Map<TheaterDto.Read>(await _repository.GetById(id));
    }

    public async Task<bool> DeleteTheater(int id)
    {
        if (await CheckExists(id))
        {
            return false;
        }
        var entity = await _repository.GetById(id);
        entity.IsActive = false;
        
        await _repository.Update(entity);
        return true;
    }
    
    public async Task<bool> CheckExists(int id)
    {
        var checkTheater = await _repository.Exists(id);
        return !checkTheater;
    }
}