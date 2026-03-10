namespace RestaurantFoods.Dtos.Profiles;

public record ProfileDto(
    Guid Id,
    Guid UserId,
    string? Gender,
    DateTime? BirthDate,
    string? PlaceOfBirth,
    string? PhoneNumber,
    string? Address
);