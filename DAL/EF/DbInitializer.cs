using Microsoft.EntityFrameworkCore;

namespace DAL.EF;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.Migrate();
    }
}