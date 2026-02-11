using System.ComponentModel.DataAnnotations;

namespace RestaurantFoods.Dtos.Auth;

public record RegisterDto(
    [Required, StringLength(50)]
    string Name,

    [Required, StringLength(50)]
    string Username,

    [Required, EmailAddress]
    string Email,

    [Required, MinLength(8)]
    string Password
);