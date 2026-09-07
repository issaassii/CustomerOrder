using CustomerOrder.Domain.DTOs;

namespace CustomerOrder.Domain.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginDto dto);
}