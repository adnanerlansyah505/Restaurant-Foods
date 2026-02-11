using RestaurantFoods.Dtos.Auth;
using RestaurantFoods.Dtos.Users;
using RestaurantFoods.Models;
using RestaurantFoods.Models.Data;
using RestaurantFoods.Repositories.Interfaces;
using RestaurantFoods.Services.Interfaces;
using RestaurantFoods.Services.Security;
using RestaurantFoods.Constants;

namespace RestaurantFoods.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher _passwordHasher;

    public AuthService(
        IUserRepository userRepository,
        PasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserDto> RegisterAsync(RegisterDto dto)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Username = dto.Username,
            Email = dto.Email,
            Password = _passwordHasher.Hash(dto.Password),
            RoleId = DefaultRoles.UserId // we’ll define this
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return new UserDto(user.Id, user.Name, user.Username, user.Email, user.RoleId);
    }

    public async Task<string> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user == null ||
            !_passwordHasher.Verify(user.Password, dto.Password))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        // TEMP: return fake token (JWT next)
        return "LOGIN_SUCCESS_TOKEN";
    }

    public async Task ForgotPasswordAsync(ForgotPasswordDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null) return;

        user.EmailVerificationToken = Guid.NewGuid().ToString();
        user.EmailVerificationExpires = DateTime.UtcNow.AddHours(1);

        await _userRepository.SaveChangesAsync();

        // TODO: send email
    }
}