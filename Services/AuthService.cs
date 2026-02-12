using RestaurantFoods.Dtos.Auth;
using RestaurantFoods.Dtos.Users;
using RestaurantFoods.Models;
using RestaurantFoods.Models.Data;
using RestaurantFoods.Repositories.Interfaces;
using RestaurantFoods.Services.Interfaces;
using RestaurantFoods.Services.Security;
using RestaurantFoods.Constants;
using RestaurantFoods.Exceptions.Identity;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;         
using System.Text;

namespace RestaurantFoods.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly PasswordHasher _passwordHasher;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;

    public AuthService(
        IUserRepository userRepository,
        IProfileRepository profileRepository,
        PasswordHasher passwordHasher,
        IConfiguration configuration,
        IEmailService emailService
    )
    {
        _userRepository = userRepository;
        _profileRepository = profileRepository;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
        _emailService = emailService;
    }

    public async Task<UserDto> RegisterAsync(RegisterDto dto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);

        if (existingUser != null)
            throw new EmailAlreadyExistsException();

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Username = dto.Username,
            Email = dto.Email,
            Password = _passwordHasher.Hash(dto.Password),
            RoleId = DefaultRoles.UserId,
            IsEmailVerified = false,
            EmailVerificationToken = Guid.NewGuid().ToString(),
            EmailVerificationExpiredAt = DateTime.UtcNow.AddMinutes(15)
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
        
        // Auto create profile
        var profile = new Profile
        {
            UserId = user.Id
        };

        await _profileRepository.AddAsync(profile);
        await _profileRepository.SaveChangesAsync();
        
        var verificationLink =
            $"{_configuration["AppSettings:BaseUrl"]}/auth/verify-email?token={user.EmailVerificationToken}";

        await _emailService.SendEmailAsync(
            user.Email,
            "Verify Your Account",
            $"""
            <h3>Email Verification</h3>
            <p>Click the link below to verify your account:</p>
            <a href="{verificationLink}">Verify Account</a>
            <br>OR<br>
            <p>{verificationLink}</p>
            <p>This link expires in 15 minutes.</p>
            """
        );

        return new UserDto(user.Id, user.Name, user.Username, user.Email);
    }

    public async Task<string?> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user == null ||
            !_passwordHasher.Verify(user.Password, dto.Password))
        {
            return null;
        }

        return GenerateJwtToken(user);
    }

    public async Task ForgotPasswordAsync(ForgotPasswordDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null) return;

        var otp = new Random().Next(100000, 999999).ToString();

        user.OtpCode = otp;
        user.OtpExpiredAt = DateTime.UtcNow.AddMinutes(1);

        await _userRepository.SaveChangesAsync();

        // TODO: kirim email
        Console.WriteLine($"OTP for {user.Email} = {otp}");
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user == null ||
            user.OtpCode != dto.OtpCode ||
            user.OtpExpiredAt < DateTime.UtcNow)
        {
            return false;
        }

        user.Password = _passwordHasher.Hash(dto.NewPassword);
        user.OtpCode = null;
        user.OtpExpiredAt = null;

        await _userRepository.SaveChangesAsync();

        return true;
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.Slug)
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public async Task<bool> VerifyEmailAsync(string token)
    {
        var user = await _userRepository.GetByVerificationTokenAsync(token);

        if (user == null ||
            user.EmailVerificationExpiredAt < DateTime.UtcNow)
        {
            return false;
        }

        user.IsEmailVerified = true;
        user.EmailVerificationToken = null;
        user.EmailVerificationExpiredAt = null;

        await _userRepository.SaveChangesAsync();

        return true;
    }

}