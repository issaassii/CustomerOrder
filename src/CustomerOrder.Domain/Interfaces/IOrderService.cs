using CustomerOrder.Domain.DTOs;

namespace CustomerOrder.Domain.Interfaces;

public interface IOrderService
{
    Task<OrderResponseDto?> GetByIdAsync(int id);
    Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto dto);
}