using CustomerService.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Data;

public class CustomerDBContext : DbContext
{
    public CustomerDBContext(DbContextOptions<CustomerDBContext> options) : base(options)
    {
    }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<OutboxEvent> OutboxEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("customer");
    }
}
