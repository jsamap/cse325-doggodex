using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DoggoDex.Models;

public class DoggoDbContext : IdentityDbContext<ApplicationUser>
{
    public DoggoDbContext(DbContextOptions<DoggoDbContext> options) : base(options) { }

    public DbSet<DogOwner> DogOwners { get; set; }
    public DbSet<BusinessOwner> BusinessOwners { get; set; }
    public DbSet<Dog> Dogs { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Keep existing One-to-One constraints
        // One-to-one between user tables to add fields to the framework user.
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

        builder.Entity<Review>()
            .HasOne(r => r.Dog)
            .WithMany(d => d.Reviews)
            .HasForeignKey(r => r.DogId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Review>()
            .HasOne(r => r.BusinessOwner)
            .WithMany(b => b.Reviews)
            .HasForeignKey(r => r.BusinessOwnerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
