using RestaurantFoods.Models.Data;

namespace RestaurantFoods.Repositories.Interfaces;

public interface IPaymentRepository
{
    Task<Payment?> GetPaymentByOrderIdAsync(Guid orderId);
    Task<Payment> CreatePaymentAsync(Guid orderId);
    Task<Order?> GetOrderWithItemsAsync(Guid orderId);
    Task UpdateOrderStatusAsync(Order order);
    Task SaveChangesAsync(); 
}
