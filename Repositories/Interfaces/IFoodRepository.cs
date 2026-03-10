using RestaurantFoods.Models.Data;

namespace RestaurantFoods.Repositories.Interfaces;

public interface IFoodRepository
{
    IQueryable<Food> Query();
    Task<IEnumerable<Food>> GetAllAsync();
    Task<Food?> GetByIdAsync(Guid id);
    Task AddAsync(Food food);
    Task DeleteAsync(Food food);
    Task SaveChangesAsync();

}
