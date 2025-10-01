using AutoMapper;
using BLL.Interfaces.Movie;
using DTO.MovieDTOS;
using Repository.DTO;
using Repository.Movie;

namespace BLL.Services.Movie;

public class MovieService : IMovieService
{
    private readonly IMapper _mapper;
    private readonly MovieRepository _movieRepository;

    public MovieService(IMapper mapper, MovieRepository movieRepository)
        {
            _movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<MovieDto.List>> GetAllMoviesAsync()
        {
            var movies = await _movieRepository.GetAll();
            return _mapper.Map<IEnumerable<MovieDto.List>>(movies);
        }

        public async Task<IEnumerable<MovieDto.List>> GetActiveMoviesAsync()
        {
            var activeMovies = await _movieRepository.GetActiveMoviesAsync();
            return _mapper.Map<IEnumerable<MovieDto.List>>(activeMovies);
        }

        public async Task<MovieDto.Read?> GetMovieByIdAsync(int id)
        {
            var movie = await _movieRepository.GetById(id);
            return movie != null ? _mapper.Map<MovieDto.Read>(movie) : null;
        }

        public async Task<MovieDto.ReadWithScreenings> GetMovieByIdWithScreenings(int id)
        {
            var movie = await _movieRepository.FectMovieWithTheaterScreenings(id);
            
            var dto = _mapper.Map<MovieDto.ReadWithScreenings>(movie);
            dto.Theaters = movie.Screenings.GroupBy(s => s.Screen.Theater)
                .Select(theater => new MovieDto.Theater
                {
                    Id = theater.Key.Id,
                    TheaterName = theater.Key.Name,
                    Screens = theater.GroupBy(s => s.Screen)
                        .Select(s => new MovieDto.Screen
                        {
                            Id = s.Key.Id,
                            ScreenName = s.Key.Name,
                            Screenings = s.Select(screening => new MovieDto.Screening
                            {
                                Id = screening.Id,
                                StartTime = screening.StartTime,
                                BasePrice = screening.BasePrice,
                            }).ToList()
                        }).ToList() 
                }).ToList();
            
            return dto;
        }
        
        public async Task<IEnumerable<CinemaDto.Theater>> GetMovieGroupedByTheater(int movieId)
        {
            var movie = await _movieRepository.FetchMovieWithScreenings(movieId);

            return movie;
        }

        public async Task<MovieDto.Read> CreateMovieAsync(MovieDto.Create createMovieDto)
        {
            if (createMovieDto == null)
                throw new ArgumentNullException(nameof(createMovieDto));

            var movie = _mapper.Map<CreateMovie>(createMovieDto);
            var createdMovie = await _movieRepository.CreateMovie(movie);
            
            return _mapper.Map<MovieDto.Read>(createdMovie);
        }

        public async Task<MovieDto.Read> UpdateMovieAsync(int id, MovieDto.Update updateMovieDto)
        {
            if (updateMovieDto == null)
                throw new ArgumentNullException(nameof(updateMovieDto));

            var existingMovie = await _movieRepository.GetById(id);
            if (existingMovie == null)
                throw new KeyNotFoundException($"Movie with ID {id} not found");

            // Map only non-null properties from DTO to existing entity
            _mapper.Map(updateMovieDto, existingMovie);
            existingMovie.UpdatedAt = DateTime.UtcNow;

            var updatedMovie = await _movieRepository.Update(existingMovie);
            return _mapper.Map<MovieDto.Read>(updatedMovie);
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            var movie = await _movieRepository.GetById(id);
            if (movie == null)
                return false;

            // Soft delete - set IsActive to false
            movie.IsActive = false;
            movie.UpdatedAt = DateTime.UtcNow;
            
            await _movieRepository.Update(movie);
            return true;
        }

        public async Task<bool> MovieExistsAsync(int id)
        {
            return await _movieRepository.Exists(id);
        }
}