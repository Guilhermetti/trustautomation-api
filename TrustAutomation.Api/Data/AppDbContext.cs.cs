using Microsoft.EntityFrameworkCore;
using TrustAutomation.Api.Models;

namespace TrustAutomation.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Lead> Leads => Set<Lead>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lead>()
            .HasIndex(x => x.CreatedAtUtc);

        modelBuilder.Entity<Lead>()
            .HasIndex(x => x.Email);
    }
}
