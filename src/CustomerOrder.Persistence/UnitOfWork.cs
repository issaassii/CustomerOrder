using CustomerOrder.Domain.Interfaces;
using CustomerOrder.Persistence.Data;
using CustomerOrder.Persistence.Repositories;

namespace CustomerOrder.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Customers = new CustomerRepository(_context);
        Users = new UserRepository(_context);
    }

    public ICustomerRepository Customers { get; }
    public IUserRepository Users { get; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}