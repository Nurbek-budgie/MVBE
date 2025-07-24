using API.Configurations;
using DTO.Auth;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController: ControllerBase
{
    private readonly AuthService _service;

    public AuthController(AuthService service)
    {
        _service = service;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(AuthDto.Register dto)
    {
        var token = await _service.RegisterUserAsync(dto);
        return Ok(new { token });
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(AuthDto.Login dto)
    {
        var token = await _service.LoginAsync(dto);
        return Ok(new { token });
    }
}