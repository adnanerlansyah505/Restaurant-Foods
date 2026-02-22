using Microsoft.EntityFrameworkCore;
using RestaurantFoods.Commons.Pagination;
using RestaurantFoods.Dtos.Users;
using RestaurantFoods.Repositories.Interfaces;
using RestaurantFoods.Services.Interfaces;
using RestaurantFoods.Models.Data;
using RestaurantFoods.Services.Security;

namespace RestaurantFoods.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly PasswordHasher _passwordHasher;

    public UserService(IUserRepository userRepository, IProfileRepository profileRepository, PasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _profileRepository = profileRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<(IEnumerable<UserDto> Users, PaginationMeta Meta)>
        GetUsersAsync(int page, int pageSize)
    {
        page = page <= 0 ? 1 : page;
        pageSize = pageSize <= 0 || pageSize > 10 ? 10 : pageSize;

        var query = _userRepository.Query();

        var totalItems = await query.CountAsync();

        var users = await query
            .Include(u => u.Profile)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserDto(
                u.Guid,
                u.Name,
                u.Username,
                u.Email,
                u.RoleId))
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
            user.Guid,
            user.Name,
            user.Email,
            user.Username,
            user.RoleId
        );
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto userDto)
    {
        var user = new User
        {
            Name = userDto.Name,
            Username = userDto.Username,
            Email = userDto.Email,
            Password = _passwordHasher.Hash(userDto.Password),
            RoleId = Guid.Parse("22222222-2222-2222-2222-222222222222")
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
        
        // Auto create profile
        var profile = new Profile
        {
            UserId = user.Guid
        };

        await _profileRepository.AddAsync(profile);
        await _profileRepository.SaveChangesAsync();

        return new UserDto(
            user.Guid,
            user.Name,
            user.Email,
            user.Username,
            user.RoleId
        );
    }

    public async Task<UserDto?> UpdateUserAsync(Guid id, UpdateUserDto userDto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;

        user.Name = userDto.Name;
        user.Username = userDto.Username;
        user.Email = userDto.Email;
        user.Password = _passwordHasher.Hash(userDto.Password);
        user.RoleId = userDto.RoleId;

        await _userRepository.SaveChangesAsync();

        return new UserDto(
            user.Guid,
            user.Name,
            user.Username,
            user.Email,
            user.RoleId);
    }

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