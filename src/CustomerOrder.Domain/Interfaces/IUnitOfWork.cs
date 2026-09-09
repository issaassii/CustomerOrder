namespace CustomerOrder.Domain.Interfaces;

public interface IUnitOfWork
{
    ICustomerRepository Customers { get; }
    IUserRepository Users { get; }
    IProductRepository Products { get; }
    Task<int> SaveChangesAsync();
    
}