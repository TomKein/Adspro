using Adspro.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adspro.Server.Infrastructure.Data;

public class AdsDbContext(DbContextOptions<AdsDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserSession> UserSessions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Login).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(512);
            entity.HasMany(e => e.UserSessions)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);
        });
        
        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.JwtToken).IsRequired();
            entity.Property(e => e.ExpiresAt).IsRequired();
        });
    }
}
