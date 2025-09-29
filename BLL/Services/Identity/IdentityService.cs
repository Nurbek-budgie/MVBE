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
        var user = _mapper.Map<User>(userDto);

        user.TheaterId = role == ERoles.Manager ? userDto.TheaterId : null;
        
        var isEmailExist = await _userManager.FindByEmailAsync(userDto.email);

        if (isEmailExist != null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "DuplicateEmail",
                Description = "A user with this email already exists."
            });
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
    
    public async Task<bool> AssignTheaterToManagerAsync(Guid managerId, int theaterId)
    {
        var user = await _userManager.FindByIdAsync(managerId.ToString());
        if (user == null)
            return false;
        
        var roles = await _userManager.GetRolesAsync(user);
        if (!roles.Contains(ERoles.Manager.ToString()))
            return false;

        user.TheaterId = theaterId;
        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded;
    }
    
    public async Task<IEnumerable<UserDto.ManagerList>> GetManagersAsync()
    {
        var users = await _userManager.GetUsersInRoleAsync(ERoles.Manager.ToString());

        return users.Select(u => new UserDto.ManagerList
        {
            Id = u.Id,
            UserName = u.UserName,
            Email = u.Email,
            TheaterId = u.TheaterId
        });
    }

}