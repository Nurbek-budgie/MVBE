using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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

    // Make create user method where it will take parameter such as user, role
    public async Task<AuthDto.Response> LoginAsync(AuthDto.Login dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.password))
            throw new UnauthorizedAccessException("Invalid credentials");

        var (userEntity, claims, roles) = await UserClaimsRole(dto.email, dto.password);
        return await BuildResponse(userEntity, claims, roles);
    }
    
    public async Task<(User user, IEnumerable<Claim> claims, IEnumerable<string> roles)> UserClaimsRole(string username, string password)
    {
        var user = await _userManager.FindByEmailAsync(username);
        // todo exceptionss
        if (user == null)
        {
            throw new Exception("User not found");
        }

        if (!await _userManager.CheckPasswordAsync(user, password))
        {
            throw new Exception("User password doesn't match");
        }
        var roles = await _userManager.GetRolesAsync(user);
        
        var claims = new List<Claim>()
        {
            new(ClaimTypes.Email, username)
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        return (user, claims, roles);
    }

    public async Task<AuthDto.Response> BuildResponse(User user, IEnumerable<Claim> claims, IEnumerable<string> roles)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var accessTokenExpires = DateTime.UtcNow.AddMinutes(15);
        var refreshTokenExpires = DateTime.UtcNow.AddDays(7);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: accessTokenExpires,
            signingCredentials: creds
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshToken = GenerateRefreshToken();

        return new AuthDto.Response
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            UserName = user.UserName,
            TokenAccessExpires = accessTokenExpires,
            RefreshTokenExpires = refreshTokenExpires,
            Roles = roles
        };
    }

    private string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}