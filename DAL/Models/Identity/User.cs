using Microsoft.AspNetCore.Identity;

namespace DAL.Models
{
    public class User : IdentityUser<Guid>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}