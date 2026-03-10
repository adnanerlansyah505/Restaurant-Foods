using System.ComponentModel.DataAnnotations;

namespace RestaurantFoods.Dtos.Profiles;

public record CreateProfileDto(
    [Required] Guid UserId,
    string? Gender,
    DateTime? BirthDate,
    string? PlaceOfBirth,
    string? PhoneNumber,
    string? Address
);