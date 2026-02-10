using RestaurantFoods.Models;
using RestaurantFoods.Dtos.Users;

namespace RestaurantFoods.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetUsersAsync();
    Task<UserDto?> GetUserByIdAsync(Guid id);
    // Task<UserDto> CreateUserAsync(User user);
    Task<bool> DeleteUserAsync(Guid id);
}
