using CustomerOrder.Domain.Interfaces;

public interface IUnitOfWork : IAsyncDisposable
{
    ICustomerRepository Customers { get; }
    IUserRepository Users { get; }
    IProductRepository Products { get; }
    IOrderRepository Orders { get; }
    IOrderReportsRepository OrderReports { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}