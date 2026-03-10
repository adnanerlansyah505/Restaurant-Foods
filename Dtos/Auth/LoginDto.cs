using System.ComponentModel.DataAnnotations;

namespace RestaurantFoods.Dtos.Auth;

public record LoginDto(
    [Required, EmailAddress]
    string Email,

    [Required]
    string Password
);
