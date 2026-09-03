using Microsoft.EntityFrameworkCore;
using CustomerOrder.Domain.Entities;

namespace CustomerOrder.Persistence.Data;

public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<User> Users => Set<User>();
}