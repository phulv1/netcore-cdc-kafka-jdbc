using IdentityService.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Data;

public class IdentityDBContext : DbContext
{
    public IdentityDBContext(DbContextOptions<IdentityDBContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<OutboxEvent> OutboxEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("identity");
    }
}
