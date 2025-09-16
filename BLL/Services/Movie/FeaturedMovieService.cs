using AutoMapper;
using BLL.Interfaces.Movie;
using DAL.Models.Movie;
using DTO.MovieDTOS;
using Repository.Movie;

namespace BLL.Services.Movie;

public class FeaturedMovieService : IFeaturedMovieService
{
    private readonly FeaturedMovieRepository _repository;
    private readonly IMapper _mapper;

    public FeaturedMovieService(FeaturedMovieRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<FeaturedMovieDto.Read> AddFeaturedMovie(FeaturedMovieDto.Create featuredMovieDto)
    {
        var  featuredMovie = _mapper.Map<FeaturedMovie>(featuredMovieDto);
        
        var movie = await _repository.Create(featuredMovie);
        
        return _mapper.Map<FeaturedMovieDto.Read>(movie);
    }
    
    public async Task<FeaturedMovieDto.Read> ChangePositionMovie(int id, FeaturedMovieDto.Update featuredMovieDto)
    {
        var  featuredMovie = await  _repository.GetById(id);
        if (featuredMovie == null)
        {
            throw new NullReferenceException($"Featured Movie with id {id} was not found");
        }
        
        featuredMovie.Position = featuredMovieDto.Position;
        await _repository.Update(featuredMovie);
        
        return _mapper.Map<FeaturedMovieDto.Read>(featuredMovie);
    }
    
    public async Task<IEnumerable<FeaturedMovieDto.Read>> ListOfFeaturedMovies()
    {
        var movie = await _repository.GetAll();
        return _mapper.Map<IEnumerable<FeaturedMovieDto.Read>>(movie);
    }

    public async Task<bool> RemoveFeaturedMovie(int id)
    {
        var movie = await _repository.GetById(id);
        if (movie == null) return false;
        await _repository.Delete(id);
        
        return true;
    }

    public async Task<bool> RemoveAllFeaturedMovies()
    {
        var movies = await _repository.DeleteAllAsync();
        return movies;
    }
}