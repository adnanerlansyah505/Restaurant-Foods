namespace RestaurantFoods.Dtos.Users;

public record UserDto(
    Guid Id,
    string Name,
    string Email
);