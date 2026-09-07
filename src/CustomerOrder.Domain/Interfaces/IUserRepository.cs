using CustomerOrder.Domain.Entities;

namespace CustomerOrder.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task AddAsync(User user);
}