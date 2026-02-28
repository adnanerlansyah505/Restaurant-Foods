using RestaurantFoods.Models;
using RestaurantFoods.Models.Data;

namespace RestaurantFoods.Services.Interfaces;

public interface IPaymentService
{
    Task<string> CreateTransactionAsync(Guid orderId, Guid userId);
    Task HandleNotificationAsync(string json);
}
