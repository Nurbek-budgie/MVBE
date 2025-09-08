using AutoMapper;
using Common.Enums.MovieEnums;
using DAL.Models;
using DAL.Models.Movie;
using DTO.Auth;
using DTO.MovieDTOS;
using DTO.Reservation;

namespace BLL.Configurations;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // User
        CreateMap<User, UserDto.Register>()
            .ForMember(x => x.email, y => y.MapFrom(z => z.Email))
            .ForMember(x => x.username, y => y.MapFrom(z => z.UserName))
            .ReverseMap()
            .AfterMap((uDTO, uModel) => uModel.NormalizedEmail = uDTO.email.ToUpper())
            .AfterMap((uDTO, uModel) => uModel.NormalizedUserName = uDTO.username.ToUpper());

        // Movie
        CreateMap<MovieDto.Create, Movie>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Screenings, opt => opt.Ignore())
            .ReverseMap();
        
        CreateMap<MovieDto.Read, Movie>()
            .ReverseMap();

        CreateMap<MovieDto.List, Movie>()
            .ReverseMap();
        
        CreateMap<MovieDto.Update, Movie>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Screenings, opt => opt.Ignore())
            .ReverseMap();
        
        // Theater
        CreateMap<TheaterDto.Create, Theater>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Screens, opt => opt.Ignore())
            .ReverseMap();
        
        CreateMap<TheaterDto.List, Theater>()
            .ReverseMap();
        
        CreateMap<TheaterDto.Update, Theater>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Screens, opt => opt.Ignore())
            .ReverseMap();
        
        CreateMap<TheaterDto.Read, Theater>()
            .ForMember(dest => dest.Screens, opt => opt.Ignore())
            .ReverseMap();
        
        // Screen
        CreateMap<ScreenDto.Create, Screen>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Theater, opt => opt.Ignore())
            .ForMember(dest => dest.Seats, opt => opt.Ignore())
            .ForMember(dest => dest.Screenings, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<ScreenDto.Update, Screen>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Theater, opt => opt.Ignore())
            .ForMember(dest => dest.Seats, opt => opt.Ignore())
            .ForMember(dest => dest.Screenings, opt => opt.Ignore())
            .ReverseMap();
        
        CreateMap<ScreenDto.Read, Screen>()
            .ReverseMap();
        
        CreateMap<ScreenDto.List, Screen>()
            .ReverseMap();
        
        //Screening
        
        CreateMap<ScreeningDto.Create, Screening>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Movie, opt => opt.Ignore())
            .ForMember(dest => dest.Screen, opt => opt.Ignore())
            .ForMember(dest => dest.Reservations, opt => opt.Ignore())
            .ReverseMap();
        
        CreateMap<ScreeningDto.Update, Screening>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Movie, opt => opt.Ignore())
            .ForMember(dest => dest.Screen, opt => opt.Ignore())
            .ForMember(dest => dest.Reservations, opt => opt.Ignore())
            .ReverseMap();
        
        CreateMap<ScreeningDto.Read, Screening>()
            .ReverseMap();
        
        CreateMap<ScreeningDto.List, Screening>()
            .ReverseMap();
        
        // Reservation

        CreateMap<ReservationDto.Create, Reservation>()
            .ForMember(dest => dest.ReservationNumber, opt => opt.Ignore())
            .ForMember(dest => dest.BookingStatus, opt => opt.MapFrom(src => BookingStatus.Pending))
            .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => PaymentStatus.Pending))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ReverseMap();
    
        CreateMap<ReservationDto.Read, Reservation>()
            .ReverseMap();
    }
}