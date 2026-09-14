using CustomerOrder.Business.Services;
using CustomerOrder.Domain.DTOs;
using CustomerOrder.Domain.Entities;
using CustomerOrder.Domain.Interfaces;
using Moq;
using Xunit;

namespace CustomerOrder.Business.Tests;

public class CustomerServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly CustomerService _sut;

    public CustomerServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _customerRepositoryMock = new Mock<ICustomerRepository>();

        _unitOfWorkMock.Setup(u => u.Customers).Returns(_customerRepositoryMock.Object);

        _sut = new CustomerService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCustomerExists_ReturnsMappedDto()
    {
        // Arrange
        var customer = new Customer
        {
            Id = 1,
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane@example.com",
            CreatedDate = DateTime.UtcNow,
            IsActive = true
        };

        _customerRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(customer);

        // Act
        var result = await _sut.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Jane", result!.FirstName);
        Assert.Equal("jane@example.com", result.Email);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCustomerDoesNotExist_ReturnsNull()
    {
        // Arrange
        _customerRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Customer?)null);

        // Act
        var result = await _sut.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ValidDto_CallsSaveChangesAndReturnsResponseWithGeneratedId()
    {
        // Arrange
        var dto = new CreateCustomerDto
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "john@example.com",
            PhoneNumber = "555-1234"
        };

        _customerRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Customer>()))
            .Returns(Task.CompletedTask)
            .Callback<Customer>(c => c.Id = 42);

        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _sut.CreateAsync(dto);

        // Assert
        Assert.Equal(42, result.Id);
        Assert.Equal("John", result.FirstName);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}