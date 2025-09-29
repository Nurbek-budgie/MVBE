using BLL.Interfaces.Identity;
using Common.Enums;
using DTO.Auth;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Auth;

[ApiController]
public class UserController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public UserController(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    // admin forgot password, change password, change data, delete admin.
    [HttpPost("RegisterClient")]
    public async Task<IActionResult> CreateClient(UserDto.Register userDto)
    {
        var result = await _identityService.CreateUserAsync(userDto, ERoles.Audience);
        return Ok(result);
    }
}