using Microsoft.EntityFrameworkCore;
using RestaurantFoods.Models;
using RestaurantFoods.Models.Data;
using RestaurantFoods.Constants;

namespace RestaurantFoods.Data.Seeders;

public static class RoleSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Roles.AnyAsync())
            return;

        var roles = new List<Role>
        {
            new()
            {
                Id = AppRoles.AdminId,
                Name = "Admin",
                Slug = "admin"
            },
            new()
            {
                Id = AppRoles.UserId,
                Name = "User",
                Slug = "user"
            },
            new()
            {
                Id = AppRoles.ChefId,
                Name = "Chef",
                Slug = "chef"
            }
        };

        await context.Roles.AddRangeAsync(roles);
        await context.SaveChangesAsync();
    }
}
