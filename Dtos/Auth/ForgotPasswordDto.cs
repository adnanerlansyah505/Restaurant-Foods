using System.ComponentModel.DataAnnotations;

namespace RestaurantFoods.Dtos.Auth;

public record ForgotPasswordDto(
    [Required, EmailAddress]
    string Email
);
