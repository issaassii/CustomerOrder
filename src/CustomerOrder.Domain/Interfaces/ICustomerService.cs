using CustomerOrder.Domain.DTOs;

namespace CustomerOrder.Domain.Interfaces;

public interface ICustomerService
{
    Task<CustomerResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<CustomerResponseDto>> GetAllAsync();
    Task<CustomerResponseDto> CreateAsync(CreateCustomerDto dto);
    Task<bool> UpdateAsync(int id, UpdateCustomerDto dto);
    Task<bool> DeleteAsync(int id);
}