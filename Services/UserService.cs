using Microsoft.EntityFrameworkCore;
using RestaurantFoods.Commons.Pagination;
using RestaurantFoods.Dtos.Users;
using RestaurantFoods.Repositories.Interfaces;
using RestaurantFoods.Services.Interfaces;

namespace RestaurantFoods.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<(IEnumerable<UserDto> Users, PaginationMeta Meta)>
        GetUsersAsync(int page, int pageSize)
    {
        page = page <= 0 ? 1 : page;
        pageSize = pageSize <= 0 || pageSize > 10 ? 10 : pageSize;

        var query = _userRepository.Query();

        var totalItems = await query.CountAsync();

        var users = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserDto(u.Id, u.Name, u.Email))
            .ToListAsync();

        var meta = new PaginationMeta
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
        };

        return (users, meta);
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