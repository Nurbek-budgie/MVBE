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
        
        // Optional: Only needed if creating a Manager
        public int? TheaterId { get; set; } 
    }
    
    public class Login
    {
        [EmailAddress]
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
    
    public class ManagerList
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int? TheaterId { get; set; }
    }
}