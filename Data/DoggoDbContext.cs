using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DoggoDex.Models;

public class DoggoDbContext : IdentityDbContext<ApplicationUser>
{
    public DoggoDbContext(DbContextOptions<DoggoDbContext> options) : base(options) { }

    public DbSet<DogOwner> DogOwners { get; set; }
    public DbSet<BusinessOwner> BusinessOwners { get; set; }
    public DbSet<Dog> Dogs { get; set; } // Added
    public DbSet<Review> Reviews { get; set; } // Added

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Keep existing One-to-One constraints
        builder.Entity<ApplicationUser>()
            .HasOne(u => u.DogOwnerProfile)
            .WithOne(p => p.IdentityUser)
            .HasForeignKey<DogOwner>(p => p.IdentityUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ApplicationUser>()
            .HasOne(u => u.BusinessOwnerProfile)
            .WithOne(p => p.IdentityUser)
            .HasForeignKey<BusinessOwner>(p => p.IdentityUserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure explicit relationships for Review to prevent multi-path cascades
        builder.Entity<Review>()
            .HasOne(r => r.Dog)
            .WithMany(d => d.Reviews)
            .HasForeignKey(r => r.DogId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Review>()
            .HasOne(r => r.BusinessOwner)
            .WithMany(b => b.Reviews)
            .HasForeignKey(r => r.BusinessOwnerId)
            .OnDelete(DeleteBehavior.Restrict); // Prevents circular delete issues in Postgres
    }
}
