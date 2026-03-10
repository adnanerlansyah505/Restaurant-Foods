using Microsoft.EntityFrameworkCore;
using RestaurantFoods.Data;
using RestaurantFoods.Models;
using RestaurantFoods.Models.Data;
using RestaurantFoods.Repositories.Interfaces;

namespace RestaurantFoods.Repositories;

public class FoodRepository : IFoodRepository
{
    private readonly AppDbContext _context;

    public FoodRepository(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<Food> Query()
    {
        return _context.Foods.AsNoTracking();
    }

    public async Task<IEnumerable<Food>> GetAllAsync()
    {
        return await _context.Foods
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Food?> GetByIdAsync(Guid id)
    {
        return await _context.Foods
            .FirstOrDefaultAsync(u => u.Guid == id);
    }

    public async Task AddAsync(Food food)
    {
        await _context.Foods.AddAsync(food);
    }

    public Task DeleteAsync(Food food)
    {
        _context.Foods.Remove(food);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
