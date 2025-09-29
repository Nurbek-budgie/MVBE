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
    [HttpPost("RegisterAccount")]
    public async Task<IActionResult> CreateAdmin(UserDto.Register userDto, ERoles role)
    {
        var result = await _identityService.CreateUserAsync(userDto, role);
        return Ok(result);
    }
    
    
    [AuthorizeRole(ERoles.Admin)]
    [HttpPost("assign-theater")]
    public async Task<IActionResult> AssignTheaterToManager([FromQuery] Guid managerId, [FromQuery] int theaterId)
    {
        var success = await _identityService.AssignTheaterToManagerAsync(managerId, theaterId);

        if (!success)
            return BadRequest("Could not assign theater. Make sure the user exists and is a manager.");

        return Ok("Theater assigned successfully.");
    }
    
    [AuthorizeRole(ERoles.Admin)]
    [HttpGet("managers")]
    public async Task<IActionResult> GetManagers()
    {
        var managers = await _identityService.GetManagersAsync();
        return Ok(managers);
    }
}