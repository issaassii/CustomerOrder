using CustomerOrder.Domain.DTOs;

namespace CustomerOrder.Domain.Interfaces;

public interface IOrderReportsRepository
{
    Task<CustomerOrderSummaryDto?> GetCustomerOrderSummaryAsync(int customerId);
    Task<IEnumerable<OrderResponseDto>> SearchOrdersAsync(int? customerId, DateTime? startDate, DateTime? endDate);
}