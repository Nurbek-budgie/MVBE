using BLL.Interfaces.Identity;
using Common.Enums;
using DTO.Auth;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public UserController(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    // POST /api/users
    [HttpPost]
    public async Task<ActionResult> CreateClient([FromBody] UserDto.Register userDto)
    {
        var result = await _identityService.CreateUserAsync(userDto, ERoles.Audience);
        if (result == null) return BadRequest("Unable to create user");
        return Ok(result);
    }
}