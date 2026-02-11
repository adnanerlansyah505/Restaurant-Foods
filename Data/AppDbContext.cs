using Microsoft.EntityFrameworkCore;
using RestaurantFoods.Models;
using RestaurantFoods.Models.Data;

namespace RestaurantFoods.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Profile> Profiles => Set<Profile>();
    public DbSet<Role> Roles => Set<Role>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Role configuration
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.HasIndex(r => r.Slug)
                  .IsUnique();

            entity.Property(r => r.CreatedAt)
                  .HasDefaultValueSql("GETUTCDATE()");
        });

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.HasIndex(u => u.Email)
                  .IsUnique();

            entity.HasIndex(u => u.Username)
                  .IsUnique();

            entity.Property(u => u.CreatedAt)
                  .HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(u => u.Role)
                  .WithMany(r => r.Users)
                  .HasForeignKey(u => u.RoleId)
                  .OnDelete(DeleteBehavior.Restrict); // VERY important
        });

        // Profile configuration
        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.HasOne(p => p.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<Profile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(p => p.UserId)
                .IsUnique();
        });
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedAt = DateTime.UtcNow;

            if (entry.State == EntityState.Modified)
                entry.Entity.UpdatedAt = DateTime.UtcNow;
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}