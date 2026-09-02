using CustomerOrder.Domain.DTOs;
using CustomerOrder.Domain.Entities;
using CustomerOrder.Domain.Interfaces;

namespace CustomerOrder.Business.Services;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerResponseDto?> GetByIdAsync(int id)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id);
        if (customer == null) return null;

        return MapToResponseDto(customer);
    }

    public async Task<IEnumerable<CustomerResponseDto>> GetAllAsync()
    {
        var customers = await _unitOfWork.Customers.GetAllAsync();
        return customers.Select(MapToResponseDto);
    }

    public async Task<CustomerResponseDto> CreateAsync(CreateCustomerDto dto)
    {
        var customer = new Customer
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = "system",
            IsActive = true,
            IsDeleted = false
        };

        await _unitOfWork.Customers.AddAsync(customer);
        await _unitOfWork.SaveChangesAsync();

        return MapToResponseDto(customer);
    }

    public async Task<bool> UpdateAsync(int id, UpdateCustomerDto dto)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id);
        if (customer == null) return false;

        customer.FirstName = dto.FirstName;
        customer.LastName = dto.LastName;
        customer.Email = dto.Email;
        customer.PhoneNumber = dto.PhoneNumber;
        customer.UpdatedDate = DateTime.UtcNow;
        customer.UpdatedBy = "system";

        _unitOfWork.Customers.Update(customer);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id);
        if (customer == null) return false;

        customer.IsDeleted = true;
        _unitOfWork.Customers.Update(customer);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    private static CustomerResponseDto MapToResponseDto(Customer customer)
    {
        return new CustomerResponseDto
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            CreatedDate = customer.CreatedDate,
            IsActive = customer.IsActive
        };
    }
}