using AutoMapper;
using BLL.Interfaces.Movie;
using DAL.Models.Movie;
using DTO.MovieDTOS;
using DTO.Reservation;
using Repository.Movie;

namespace BLL.Services.Movie;

public class ScreeningService : IScreeningService
{
    private readonly ScreeningRepository _screeningRepository;
    private readonly IMapper _mapper;

    public ScreeningService(ScreeningRepository screeningRepository, TheaterRepository theaterRepository, IMapper mapper)
    {
        _screeningRepository = screeningRepository ?? throw new ArgumentNullException(nameof(screeningRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    public async Task<ScreeningDto.Read> CreateScreeningAsync(ScreeningDto.Create screeningDto)
    {
        if (screeningDto == null) throw new ArgumentNullException(nameof(screeningDto));
     
        var entity = _mapper.Map<Screening>(screeningDto);
        
        var screening = await _screeningRepository.Create(entity);
        
        return _mapper.Map<ScreeningDto.Read>(screening);
    }

    public async Task<ScreeningDto.Read?> UpdateScreeningAsync(int id, ScreeningDto.Update screeningDto)
    {
        if (await CheckExists(id))
        {
            throw new KeyNotFoundException($"Screening with id {id} was not found");
        }
        if (screeningDto == null) throw new ArgumentNullException(nameof(screeningDto));
        
        var screening = await _screeningRepository.GetById(id);
        _mapper.Map(screeningDto, screening);
        await _screeningRepository.Update(screening);
        
        return _mapper.Map<ScreeningDto.Read>(screening);
    }

    public async Task<ScreeningDto.Read> GetScreeningIdAsync(int id)
    {
        if (await CheckExists(id))
        {
            throw new KeyNotFoundException($"Screening with id {id} was not found");
        }
        
        var screening = await _screeningRepository.GetById(id);
        return _mapper.Map<ScreeningDto.Read>(screening);
    }

    public async Task<ScreenSeatDto.Result> GetScreeningSeatsIdAsync(int screeningId)
    {
        var screening =  await _screeningRepository.GetScreeningSeats(screeningId);

        if (screening == null)
        {
            throw new KeyNotFoundException($"Screening with id {screeningId} was not found");
        }
        
        var reservedIds = screening.Reservations
            .SelectMany(x => x.ReservedSeats)
            .Select(x => x.SeatId)
            .ToHashSet();


        var seat = screening.Screen.Seats
            .OrderBy(x => x.RowNumber)
            .ThenBy(x => int.Parse(x.SeatNumber))
            .Select(x => new ScreenSeatDto.Seat
            {
                SeatId = x.Id,
                Row = x.RowNumber,
                SeatNumber = x.SeatNumber,
                SeatName = string.Concat(x.RowNumber, x.SeatNumber),
                IsBooked = reservedIds.Contains(x.Id)
            })
            .ToList();

        return new ScreenSeatDto.Result
        {
            ScreeningId = screeningId,
            Row = seat.Select(x => x.Row).Distinct().ToList(),
            SeatNumber = seat.Select(x => x.SeatNumber).Distinct().ToList(),
            Seats = seat
        };
    }

    public async Task<IEnumerable<ScreeningDto.List>> GetAllActiveScreeningAsync()
    {
        var screenings = await _screeningRepository.GetActiveScreeningAsync();
        return _mapper.Map<IEnumerable<ScreeningDto.List>>(screenings);
    }

    public async Task<IEnumerable<ScreeningDto.List>> GetAllScreeningAsync()
    {
        var screenings = await _screeningRepository.GetAll();
        return _mapper.Map<IEnumerable<ScreeningDto.List>>(screenings);
    }

    public async Task<bool> DeleteScreeningAsync(int id)
    {
        if (await CheckExists(id))
        {
            throw new KeyNotFoundException($"Screening with id {id} was not found");
        }
        
        var screening = await _screeningRepository.GetById(id);
        screening.IsActive = false;
        await _screeningRepository.Update(screening);
        
        return true;
    }

    public async Task<bool> CheckExists(int id)
    {
        var checkif =  await _screeningRepository.Exists(id);
        return !checkif;
    }
}