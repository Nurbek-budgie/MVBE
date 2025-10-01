using API.Configurations;
using BLL.Interfaces.Identity;
using Common.Enums;
using DTO.Auth;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Auth;

[ApiController]
[Route("api/admin-users")]
public class AdminUserController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public AdminUserController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    // POST /api/admin-users
    [AuthorizeRole(ERoles.Admin)]
    [HttpPost]
    public async Task<ActionResult> CreateAdmin([FromBody] UserDto.Register userDto, [FromQuery] ERoles role)
    {
        var result = await _identityService.CreateUserAsync(userDto, role);

        if (result == null)
            return BadRequest("Unable to create user.");

        return Ok(result);
    }
    
    // TODO GET /api/admin-users/{id}
    // Retrieve a specific admin user
    

    // POST /api/admin-users/assign-theater
    [AuthorizeRole(ERoles.Admin)]
    [HttpPost("assign-theater")]
    public async Task<IActionResult> AssignTheaterToManager([FromQuery] Guid managerId, [FromQuery] int theaterId)
    {
        var success = await _identityService.AssignTheaterToManagerAsync(managerId, theaterId);

        if (!success)
            return BadRequest("Could not assign theater. Make sure the user exists and is a manager.");

        return Ok("Theater assigned successfully.");
    }

    // GET /api/admin-users/managers
    [AuthorizeRole(ERoles.Admin)]
    [HttpGet("managers")]
    public async Task<ActionResult> GetManagers()
    {
        var managers = await _identityService.GetManagersAsync();

        if (managers == null || !managers.Any())
            return NotFound("No managers found.");

        return Ok(managers);
    }
}
