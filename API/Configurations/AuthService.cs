using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DAL.Models;
using DTO.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Configurations;

public class AuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<User> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<string> RegisterUserAsync(AuthDto.Register userDto)
    {
        var user = new User { UserName = userDto.username, Email = userDto.email };
        var result = await _userManager.CreateAsync(user, userDto.password);

        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().Description);
        }
        return GenerateJwt(user);
    }

    public async Task<string> LoginAsync(AuthDto.Login dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.password))
            throw new UnauthorizedAccessException("Invalid credentials");

        return GenerateJwt(user);
    }
   
    private string GenerateJwt(User user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            },
            expires: DateTime.Now.AddMinutes(3),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    } 
}