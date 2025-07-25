using Common.Enums;
using DTO.Auth;
using Microsoft.AspNetCore.Identity;

namespace BLL.Interfaces.Identity;

public interface IIdentityService
{
    public Task<IdentityResult> CreateUserAsync(UserDto.Register userDto, ERoles role);
}