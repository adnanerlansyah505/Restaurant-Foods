using RestaurantFoods.Models.Data;

namespace RestaurantFoods.Repositories.Interfaces;

public interface IUserRepository
{
    IQueryable<User> Query();
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User user);
    Task DeleteAsync(User user);
    Task SaveChangesAsync();
    Task<User?> GetByEmailAsync(string email);
}
