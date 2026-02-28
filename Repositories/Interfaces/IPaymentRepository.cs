using RestaurantFoods.Models.Data;

namespace RestaurantFoods.Repositories.Interfaces;

public interface IPaymentRepository
{
    Task<Order?> GetOrderWithItemsAsync(Guid orderId);
    Task UpdateOrderStatusAsync(Order order);
    Task SaveChangesAsync(); 
}
