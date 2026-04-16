using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DAL.EF;
using DAL.Models;
using DTO.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Configurations;

public class AuthService
{
    private static readonly TimeSpan AccessTokenLifetime = TimeSpan.FromMinutes(15);
    private static readonly TimeSpan RefreshTokenLifetime = TimeSpan.FromDays(7);

    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _db;

    public AuthService(UserManager<User> userManager, IConfiguration configuration, AppDbContext db)
    {
        _userManager = userManager;
        _configuration = configuration;
        _db = db;
    }

    public async Task<AuthDto.Response> LoginAsync(AuthDto.Login dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.password))
            throw new UnauthorizedAccessException("Invalid credentials");

        return await IssueTokensAsync(user);
    }

    public async Task<AuthDto.Response> RefreshAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new UnauthorizedAccessException("Invalid refresh token");

        var hash = Hash(refreshToken);
        var existing = await _db.RefreshTokens.FirstOrDefaultAsync(rt => rt.TokenHash == hash);

        if (existing == null || existing.RevokedAt != null || existing.ExpiresAt <= DateTime.UtcNow)
            throw new UnauthorizedAccessException("Invalid refresh token");

        var user = await _userManager.FindByIdAsync(existing.UserId.ToString());
        if (user == null)
            throw new UnauthorizedAccessException("Invalid refresh token");

        existing.RevokedAt = DateTime.UtcNow;
        var response = await IssueTokensAsync(user);
        existing.ReplacedByTokenHash = Hash(response.RefreshToken);
        await _db.SaveChangesAsync();

        return response;
    }

    private async Task<AuthDto.Response> IssueTokensAsync(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var claims = BuildClaims(user, roles);

        var accessTokenExpires = DateTime.UtcNow.Add(AccessTokenLifetime);
        var accessToken = BuildAccessToken(claims, accessTokenExpires);

        var refreshPlain = GenerateRefreshToken();
        var refreshExpires = DateTime.UtcNow.Add(RefreshTokenLifetime);
        _db.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            TokenHash = Hash(refreshPlain),
            ExpiresAt = refreshExpires
        });
        await _db.SaveChangesAsync();

        return new AuthDto.Response
        {
            AccessToken = accessToken,
            RefreshToken = refreshPlain,
            UserName = user.UserName,
            TokenAccessExpires = accessTokenExpires,
            RefreshTokenExpires = refreshExpires,
            Roles = roles
        };
    }

    private List<Claim> BuildClaims(User user, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email ?? string.Empty)
        };
        if (user.TheaterId.HasValue)
            claims.Add(new Claim("theaterId", user.TheaterId.Value.ToString()));

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
            claims.Add(new Claim("role", role));
        }
        return claims;
    }

    private string BuildAccessToken(IEnumerable<Claim> claims, DateTime expires)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var bytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    private static string Hash(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash);
    }
}
