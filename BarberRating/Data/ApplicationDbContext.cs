using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BarberRating.Data.Entities;

namespace BarberRating.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Review> Reviews { get; set; }
    public DbSet<Barber> Barbers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>()
            .HasMany(u => u.Reviews)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId);

        builder.Entity<Barber>()
            .HasMany(b => b.Reviews)
            .WithOne(r => r.Barber)
            .HasForeignKey(r => r.BarberId);
    }
}
