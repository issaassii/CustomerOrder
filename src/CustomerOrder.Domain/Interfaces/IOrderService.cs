using CustomerOrder.Domain.DTOs;

namespace CustomerOrder.Domain.Interfaces;

public interface IOrderService
{
    Task<OrderResponseDto?> GetByIdAsync(int id);
    Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto dto);
    Task<CustomerOrderSummaryDto?> GetCustomerOrderSummaryAsync(int customerId);
    Task<IEnumerable<OrderResponseDto>> SearchOrdersAsync(int? customerId, DateTime? startDate, DateTime? endDate);
}