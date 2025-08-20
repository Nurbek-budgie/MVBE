using API.Configurations;
using BLL.Interfaces.Identity;
using Common.Enums;
using DTO.Auth;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Auth;

[ApiController]
public class AdminUserController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public AdminUserController(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    // admin forgot password, change password, change data, delete admin.
    [AuthorizeRole(ERoles.Admin)]
    [HttpPost("RegisterAdmin")]
    public async Task<IActionResult> CreateAdmin(UserDto.Register userDto, ERoles role)
    {
        var result = await _identityService.CreateUserAsync(userDto, role);
        return Ok(result);
    }
}