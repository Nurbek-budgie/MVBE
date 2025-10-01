using API.Configurations;
using DTO.Auth;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _service;
    
    public AuthController(AuthService service)
    {
        _service = service;
    }

    // POST /api/auth/login
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] AuthDto.Login dto)
    {
        var token = await _service.LoginAsync(dto);

        if (token == null)
            return Unauthorized("Invalid username or password.");
        
        return Ok(token);
    }
}