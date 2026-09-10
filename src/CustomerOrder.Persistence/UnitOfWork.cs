using CustomerOrder.Domain.Interfaces;
using CustomerOrder.Persistence.Data;
using CustomerOrder.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace CustomerOrder.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Customers = new CustomerRepository(_context);
        Users = new UserRepository(_context);
        Products = new ProductRepository(_context);
        Orders = new OrderRepository(_context);
    }

    public ICustomerRepository Customers { get; }
    public IUserRepository Users { get; }
    public IProductRepository Products { get; }
    public IOrderRepository Orders { get; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
        }
    }
}