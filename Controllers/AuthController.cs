using Microsoft.AspNetCore.Mvc;
using RestaurantFoods.Dtos.Auth;
using RestaurantFoods.Services.Interfaces;
using RestaurantFoods.Utilities.Handlers;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantFoods.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = await _authService.RegisterAsync(dto);

        return Ok(new ResponseHandlers<object>
        {
            Code = StatusCodes.Status201Created,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Register successful",
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await _authService.LoginAsync(dto);

        if (token == null)
        {
            return Unauthorized(new ResponseHandlers<object>
            {
                Code = StatusCodes.Status401Unauthorized,
                Status = HttpStatusCode.Unauthorized.ToString(),
                Message = "Invalid email or password"
            });
        }

        return Ok(new ResponseHandlers<object>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Login successful",
            Data = new { token }
        });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
    {
        await _authService.ForgotPasswordAsync(dto);

        return Ok(new ResponseHandlers<object>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "If the email exists, OTP has been sent"
        });
    }
    
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
    {
        var result = await _authService.ResetPasswordAsync(dto);

        if (!result)
        {
            return BadRequest(new ResponseHandlers<object>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Invalid or expired OTP"
            });
        }

        return Ok(new ResponseHandlers<object>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Password updated successfully"
        });
    }

    [HttpGet("verify-email")]
    public async Task<IActionResult> VerifyEmail(string token)
    {
        var result = await _authService.VerifyEmailAsync(token);

        if (!result)
        {
            return BadRequest(new ResponseHandlers<object>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Invalid or expired verification token"
            });
        }

        return Ok(new ResponseHandlers<object>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Email verified successfully"
        });
    }

}
