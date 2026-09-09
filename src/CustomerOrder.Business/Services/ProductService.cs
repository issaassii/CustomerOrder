using CustomerOrder.Domain.DTOs;
using CustomerOrder.Domain.Entities;
using CustomerOrder.Domain.Interfaces;

namespace CustomerOrder.Business.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductResponseDto?> GetByIdAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        return product == null ? null : MapToResponseDto(product);
    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
    {
        var products = await _unitOfWork.Products.GetAllAsync();
        return products.Select(MapToResponseDto);
    }

    public async Task<ProductResponseDto> CreateAsync(CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            StockQuantity = dto.StockQuantity,
            CreatedDate = DateTime.UtcNow,
            IsActive = true,
            IsDeleted = false
        };

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return MapToResponseDto(product);
    }

    public async Task<bool> UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null) return false;

        product.Name = dto.Name;
        product.Price = dto.Price;
        product.StockQuantity = dto.StockQuantity;

        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null) return false;

        product.IsDeleted = true;
        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    private static ProductResponseDto MapToResponseDto(Product product)
    {
        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            IsActive = product.IsActive
        };
    }
}