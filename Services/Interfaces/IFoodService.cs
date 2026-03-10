using RestaurantFoods.Models;
using RestaurantFoods.Models.Data;
using RestaurantFoods.Dtos.Foods;
using RestaurantFoods.Commons.Pagination;

namespace RestaurantFoods.Services.Interfaces;

public interface IFoodService
{
    Task<(IEnumerable<FoodDto> Foods, PaginationMeta Meta)> GetFoodsAsync(int page, int pageSize);
    Task<FoodDto?> GetFoodByIdAsync(Guid id);
    Task<FoodDto> CreateFoodAsync(SaveFoodDto foodDto);
    Task<FoodDto?> UpdateFoodAsync(Guid id, SaveFoodDto foodDto);
    Task<bool> DeleteFoodAsync(Guid id);
}
