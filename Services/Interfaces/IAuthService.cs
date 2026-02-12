using RestaurantFoods.Dtos.Auth;
using RestaurantFoods.Dtos.Users;

namespace RestaurantFoods.Services.Interfaces;

public interface IAuthService
{
    Task<UserDto> RegisterAsync(RegisterDto dto);
    Task<string?> LoginAsync(LoginDto dto);
    Task ForgotPasswordAsync(ForgotPasswordDto dto);
    Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
}