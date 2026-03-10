namespace RestaurantFoods.Dtos.Auth;

public record ResetPasswordDto(
    string Email,
    string OtpCode,
    string NewPassword
);