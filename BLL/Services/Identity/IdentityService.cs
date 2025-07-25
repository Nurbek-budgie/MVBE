using AutoMapper;
using BLL.Interfaces.Identity;
using Common.Enums;
using DAL.Models;
using DTO.Auth;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IMapper _mapper;

    public IdentityService(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<IdentityResult> CreateUserAsync(UserDto.Register userDto, ERoles role)
    {
        var user = _mapper.Map<User>(_mapper.Map<UserDto.Register>(userDto));
        var isEmailExist = await _userManager.FindByEmailAsync(userDto.email);

        if (isEmailExist != null)
        {
            return IdentityResult.Failed();
        }
        var result = await _userManager.CreateAsync(user, userDto.password);
        if (!result.Succeeded)
        {
            return IdentityResult.Failed();
        }
        await _userManager.SetLockoutEnabledAsync(user, false);
        await _userManager.AddToRoleAsync(user, role.ToString());
        
        return IdentityResult.Success;
    }
    
}