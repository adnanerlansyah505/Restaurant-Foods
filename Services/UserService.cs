using RestaurantFoods.Models;
using RestaurantFoods.Repositories.Interfaces;
using RestaurantFoods.Services.Interfaces;
using RestaurantFoods.Dtos.Users;

namespace RestaurantFoods.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> GetUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(u => new UserDto(
            u.Id,
            u.Name,
            u.Email
        ));
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
            return null;

        return new UserDto(
            user.Id,
            user.Name,
            user.Email
        );
    }

    // public async Task<UserDto> CreateUserAsync(User user)
    // {
    //     await _userRepository.AddAsync(user);
    //     await _userRepository.SaveChangesAsync();
    //     return user;
    // }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return false;

        await _userRepository.DeleteAsync(user);
        await _userRepository.SaveChangesAsync();
        return true;
    }
}
