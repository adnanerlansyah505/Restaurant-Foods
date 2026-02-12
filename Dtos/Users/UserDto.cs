using RestaurantFoods.Models.Data;

namespace RestaurantFoods.Dtos.Users;

public record UserDto(
    Guid Id,
    string Name,
    string Email,
    string Username,
    Guid? RoleId = null
);