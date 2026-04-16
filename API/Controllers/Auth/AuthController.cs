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
    public async Task<ActionResult<AuthDto.Response>> Login([FromBody] AuthDto.Login dto)
    {
        var tokens = await _service.LoginAsync(dto);
        return Ok(tokens);
    }

    // POST /api/auth/refresh
    [HttpPost("refresh")]
    public async Task<ActionResult<AuthDto.Response>> Refresh([FromBody] AuthDto.RefreshToken dto)
    {
        var tokens = await _service.RefreshAsync(dto.token);
        return Ok(tokens);
    }
}
