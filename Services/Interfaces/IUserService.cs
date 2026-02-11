using RestaurantFoods.Models;
using RestaurantFoods.Dtos.Users;
using RestaurantFoods.Commons.Pagination;

namespace RestaurantFoods.Services.Interfaces;

public interface IUserService
{
    Task<(IEnumerable<UserDto> Users, PaginationMeta Meta)> GetUsersAsync(int page, int pageSize);
    Task<UserDto?> GetUserByIdAsync(Guid id);
    // Task<UserDto> CreateUserAsync(User user);
    Task<bool> DeleteUserAsync(Guid id);
}
