using CustomerOrder.Domain.Entities;

namespace CustomerOrder.Domain.Interfaces;

public interface ICustomerRepository {
    Task<Customer?> GetByIdAsync(int id);
    Task<IEnumerable<Customer>> GetAllAsync();
    Task AddAsync(Customer customer);
    void Update(Customer customer);
    void Delete(Customer customer);
}