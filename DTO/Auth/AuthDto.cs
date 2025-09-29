using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DTO.Auth;

public class AuthDto
{
    public class Register
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
    
    public class Login
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }

    public class RefreshToken
    {
        [Required]
        public string token { get; set; }
    }
    
    public class Response
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public string UserName { get; set; } 
        
        public DateTime TokenAccessExpires { get; set; }
        
        public DateTime RefreshTokenExpires { get; set; }
        
        [JsonPropertyName("roles")]
        public IEnumerable<string> Roles { get; set; }
    }
    
    public class Jwt
    {
        public string Key { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int AccessTokenLifeTimeInMinutes { get; set; }

        public int RefreshTokenLifeTimeInMinutes { get; set; }
    }
}