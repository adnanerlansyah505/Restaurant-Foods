using Microsoft.EntityFrameworkCore;
using RestaurantFoods.Models;
using RestaurantFoods.Models.Data;
using RestaurantFoods.Constants;
using RestaurantFoods.Services.Security;

namespace RestaurantFoods.Data.Seeders;

public static class UserSeeder
{
    public static async Task SeedAsync(
        AppDbContext context,
        PasswordHasher passwordHasher)
    {
        if (await context.Users.AnyAsync())
            return;

        var admin = new User
        {
            Guid = Guid.NewGuid(),
            Name = "System Admin",
            Username = "admin",
            Email = "admin@restaurantfoods.com",
            Password = passwordHasher.Hash("Admin@123"),
            RoleId = AppRoles.AdminId,
            IsEmailVerified = true
        };

        var user = new User
        {
            Guid = Guid.NewGuid(),
            Name = "Demo User",
            Username = "user",
            Email = "user@restaurantfoods.com",
            Password = passwordHasher.Hash("User@123"),
            RoleId = AppRoles.UserId,
            IsEmailVerified = true
        };

        await context.Users.AddRangeAsync(admin, user);
        await context.SaveChangesAsync();
    }
}
