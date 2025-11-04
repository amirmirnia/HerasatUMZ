using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Domain.Entities.Users;
using Domain.Enum;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Interfaces;

namespace Infrastructure.Data;

public static class ApplicationDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext context, ILogger<ApplicationDbContext> logger, IPasswordHasher passwordHasher)
    {
        try
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();
            
            // Seed default users
            await SeedUsersAsync(context, logger, passwordHasher);
            
            logger.LogInformation("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    private static async Task SeedUsersAsync(ApplicationDbContext context, ILogger<ApplicationDbContext> logger, IPasswordHasher passwordHasher)
    {
        // Check if users already exist
        if (await context.Users.AnyAsync())
        {
            logger.LogInformation("Users already exist, skipping user seeding");
            return;
        }

        var users = new List<User>
        {
            new User
            {
                FirstName = "سید امیرحسین ",
                LastName = "میرنیا",
                Email = "s.amirmirnia@gmail.com",
                Phone = "09114596785",
                IdCode="2050640791",
                PasswordHash = passwordHasher.HashPassword("Qazwqazwamir01"),
                Company = "دانشگاه مازندران",
                JobTitle = "مرکز فناوری اطلاعات",
                Role = UserRole.Admin,
                IsEmailVerified = true,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = "System",
                IsActive=true
            },

        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();
        
        logger.LogInformation($"Seeded {users.Count} default users");
    }

   
}