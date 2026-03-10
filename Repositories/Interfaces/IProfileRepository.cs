using RestaurantFoods.Models;
using RestaurantFoods.Models.Data;

namespace RestaurantFoods.Repositories.Interfaces;

public interface IProfileRepository
{
    Task<Profile?> GetByUserIdAsync(Guid userId);
    Task AddAsync(Profile profile);
    Task SaveChangesAsync();
}