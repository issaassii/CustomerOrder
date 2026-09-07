using CustomerOrder.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CustomerOrder.Persistence.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context, string adminPassword)
    {
        if (await context.Users.AnyAsync()) return;

        var hasher = new PasswordHasher<object>();

        var adminUser = new User
        {
            Username = "admin",
            Role = "Admin",
            CreatedDate = DateTime.UtcNow,
            IsActive = true
        };
        adminUser.PasswordHash = hasher.HashPassword(new object(), adminPassword);

        await context.Users.AddAsync(adminUser);
        await context.SaveChangesAsync();
    }
}