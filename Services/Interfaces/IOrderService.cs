using RestaurantFoods.Models;
using RestaurantFoods.Models.Data;
using RestaurantFoods.Dtos.Orders;
using RestaurantFoods.Commons.Pagination;

namespace RestaurantFoods.Services.Interfaces;

public interface IOrderService
{
    Task<object> CreateOrderAsync(Guid userId, CreateOrderDto dto);
}