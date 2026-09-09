using CustomerOrder.Domain.DTOs;

namespace CustomerOrder.Domain.Interfaces;

public interface IProductService
{
    Task<ProductResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<ProductResponseDto>> GetAllAsync();
    Task<ProductResponseDto> CreateAsync(CreateProductDto dto);
    Task<bool> UpdateAsync(int id, UpdateProductDto dto);
    Task<bool> DeleteAsync(int id);
}