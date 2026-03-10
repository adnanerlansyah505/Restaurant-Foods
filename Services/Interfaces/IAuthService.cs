using RestaurantFoods.Dtos.Auth;
using RestaurantFoods.Dtos.Users;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;

namespace RestaurantFoods.Services.Interfaces;

public interface IAuthService
{
    Task<UserDto> RegisterAsync(RegisterDto dto);
    Task<string?> LoginAsync(LoginDto dto);
    Task<string?> GoogleLoginAsync(GoogleJsonWebSignature.Payload payload);
    Task ForgotPasswordAsync(ForgotPasswordDto dto);
    Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
    Task<bool> VerifyEmailAsync(string token);
}