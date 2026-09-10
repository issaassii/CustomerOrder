using CustomerOrder.Domain.Entities;

namespace CustomerOrder.Domain.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdWithItemsAsync(int id);
    Task AddAsync(Order order);
}