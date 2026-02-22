using RestaurantFoods.Models.Data;

namespace RestaurantFoods.Dtos.Foods;

public record FoodDto(
    Guid Id,
    string Name,
    string? Description,
    int Price,
    int? Cost = null,
    Guid? CategoryId = null
);