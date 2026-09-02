namespace CustomerOrder.Domain.Interfaces;

public interface IUnitOfWork
{
    ICustomerRepository Customers { get; }
    Task<int> SaveChangesAsync();
}