using AutoMapper;
using DAL.Models;
using DTO.Auth;

namespace BLL.Configurations;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserDto.Register>()
            .ForMember(x => x.email, y => y.MapFrom(z => z.Email))
            .ForMember(x => x.username, y => y.MapFrom(z => z.UserName))
            .ReverseMap()
            .AfterMap((uDTO, uModel) => uModel.NormalizedEmail = uDTO.email.ToUpper())
            .AfterMap((uDTO, uModel) => uModel.NormalizedUserName = uDTO.username.ToUpper());
    }
}