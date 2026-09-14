using CustomerOrder.Business.Services;
using CustomerOrder.Domain.DTOs;
using CustomerOrder.Domain.Entities;
using CustomerOrder.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace CustomerOrder.Business.Tests;

public class AuthServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _configurationMock = new Mock<IConfiguration>();

        _unitOfWorkMock.Setup(u => u.Users).Returns(_userRepositoryMock.Object);

        _configurationMock.Setup(c => c["Jwt:Key"]).Returns("test-signing-key-at-least-32-characters-long");
        _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
        _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");
        _configurationMock.Setup(c => c["Jwt:ExpiryMinutes"]).Returns("60");

        _sut = new AuthService(_unitOfWorkMock.Object, _configurationMock.Object);
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsTokenAndExpiry()
    {
        // Arrange
        var hasher = new PasswordHasher<object>();
        var storedHash = hasher.HashPassword(new object(), "CorrectPassword123");

        var user = new User
        {
            Id = 1,
            Username = "testuser",
            PasswordHash = storedHash,
            Role = "User",
            IsActive = true
        };

        _userRepositoryMock
            .Setup(r => r.GetByUsernameAsync("testuser"))
            .ReturnsAsync(user);

        var loginDto = new LoginDto { Username = "testuser", Password = "CorrectPassword123" };

        // Act
        var result = await _sut.LoginAsync(loginDto);

        // Assert
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result!.Token));
        Assert.True(result.ExpiresAt > DateTime.UtcNow);
    }

    [Fact]
    public async Task LoginAsync_WrongPassword_ReturnsNull()
    {
        // Arrange
        var hasher = new PasswordHasher<object>();
        var storedHash = hasher.HashPassword(new object(), "CorrectPassword123");

        var user = new User
        {
            Id = 1,
            Username = "testuser",
            PasswordHash = storedHash,
            Role = "User",
            IsActive = true
        };

        _userRepositoryMock
            .Setup(r => r.GetByUsernameAsync("testuser"))
            .ReturnsAsync(user);

        var loginDto = new LoginDto { Username = "testuser", Password = "WrongPassword" };

        // Act
        var result = await _sut.LoginAsync(loginDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_UsernameDoesNotExist_ReturnsNull()
    {
        // Arrange
        _userRepositoryMock
            .Setup(r => r.GetByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        var loginDto = new LoginDto { Username = "ghost", Password = "AnyPassword" };

        // Act
        var result = await _sut.LoginAsync(loginDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_InactiveUser_ReturnsNull()
    {
        // Arrange
        var hasher = new PasswordHasher<object>();
        var storedHash = hasher.HashPassword(new object(), "CorrectPassword123");

        var user = new User
        {
            Id = 1,
            Username = "testuser",
            PasswordHash = storedHash,
            Role = "User",
            IsActive = false
        };

        _userRepositoryMock
            .Setup(r => r.GetByUsernameAsync("testuser"))
            .ReturnsAsync(user);

        var loginDto = new LoginDto { Username = "testuser", Password = "CorrectPassword123" };

        // Act
        var result = await _sut.LoginAsync(loginDto);

        // Assert
        Assert.Null(result);
    }
}