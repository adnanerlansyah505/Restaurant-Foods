using Microsoft.EntityFrameworkCore;
using RestaurantFoods.Data;
using RestaurantFoods.Models;
using RestaurantFoods.Models.Data;
using RestaurantFoods.Repositories.Interfaces;

namespace RestaurantFoods.Repositories;

public class ProfileRepository : IProfileRepository
{
    private readonly AppDbContext _context;

    public ProfileRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Profile?> GetByUserIdAsync(Guid userId)
    {
        return await _context.Profiles
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task AddAsync(Profile profile)
    {
        await _context.Profiles.AddAsync(profile);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
