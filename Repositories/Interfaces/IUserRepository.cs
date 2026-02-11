using RestaurantFoods.Models;

namespace RestaurantFoods.Repositories.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User user);
    Task DeleteAsync(User user);
    Task SaveChangesAsync();
    Task<User?> GetByEmailAsync(string email);
}
