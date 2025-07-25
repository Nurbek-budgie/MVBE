using System.ComponentModel.DataAnnotations;

namespace DTO.Auth;

public class UserDto
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
}