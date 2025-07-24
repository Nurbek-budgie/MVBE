using Microsoft.AspNetCore.Identity;

namespace DAL.Models;

public class Role : IdentityRole<Guid>
{
    public virtual ICollection<UserRole> UserRoles { get; set; }
}