using Microsoft.AspNetCore.Identity;

namespace DAL.Models
{
    public class User : IdentityUser<Guid>
    {
        public int? TheaterId { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}