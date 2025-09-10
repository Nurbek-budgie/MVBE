using DAL.Models;
using DAL.Models.Movie;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF;

public class AppDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    
    }
    
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Theater> Theaters { get; set; }
    public DbSet<Screen> Screens { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Screening> Screenings { get; set; }
    public DbSet<ScreeningSeatPrice> ScreeningSeatPrices { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<ReservedSeat> ReservedSeats { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>().ToTable("Users");
        builder.Entity<Role>().ToTable("Roles");
        builder.Entity<UserClaim>().ToTable("UserClaims");
        builder.Entity<UserRole>().ToTable("UserRoles");
        builder.Entity<UserLogin>().ToTable("UserLogins");
        builder.Entity<RoleClaim>().ToTable("RoleClaims");
        builder.Entity<UserToken>().ToTable("UserTokens");
        
        builder.Entity<UserRole>(userRole =>
        {
            userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

            userRole.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            userRole.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        });

        builder.Entity<Seat>()
            .HasIndex(s => new { s.ScreenId, s.RowNumber, s.SeatNumber })
            .IsUnique();
        
        builder.Entity<Reservation>()
            .HasIndex(r => new {r.ReservationNumber})
            .IsUnique();
        
        builder.Entity<Movie>()
            .HasIndex(m => m.Title);
            
        builder.Entity<Movie>()
            .HasIndex(m => m.Genre);
            
        builder.Entity<Movie>()
            .HasIndex(m => m.ReleaseDate);
            
        builder.Entity<Screening>()
            .HasIndex(s => s.StartTime);
            
        builder.Entity<Reservation>()
            .HasIndex(r => r.UserId);

        builder.Entity<ReservedSeat>()
            .HasOne(r => r.Reservation)
            .WithMany(r => r.ReservedSeats)
            .HasForeignKey(r => r.ReservationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}