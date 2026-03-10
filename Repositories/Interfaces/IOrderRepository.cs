using RestaurantFoods.Models.Data;
using RestaurantFoods.Utilities.Enum;

namespace RestaurantFoods.Repositories.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task<Order?> GetByIdAsync(Guid orderId);
    Task<Order?> GetByIdAndUserAsync(Guid orderId, Guid userId);
    Task UpdateStatusAsync(Guid orderId, TypeStatusOrder status);
    Task SaveChangesAsync();
}