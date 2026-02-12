using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore;
using RestaurantFoods.Data;
using RestaurantFoods.Models;
using RestaurantFoods.Services;
using RestaurantFoods.Utilities.Handlers;
using RestaurantFoods.Dtos.Users;
using RestaurantFoods.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantFoods.Controllers;

[Authorize(Roles = "admin")]
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var (users, meta) = await _userService.GetUsersAsync(page, pageSize);

        if (users == null)
        {
            return NotFound(new ResponseHandlers<IEnumerable<UserDto>>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Users not found",
            });
        }

        return Ok(new ResponseHandlers<IEnumerable<UserDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Users retrieved successfully",
            Data = users,
            Meta = meta
        });
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);

        if (user == null)
            return NotFound(new ResponseHandlers<UserDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "User not found"
            });

        return Ok(new ResponseHandlers<UserDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "User retrieved successfully",
            Data = user
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDto userDto)
    {
        var user = await _userService.CreateUserAsync(userDto);

        return Ok(new ResponseHandlers<UserDto>
        {
            Code = StatusCodes.Status201Created,
            Status = HttpStatusCode.Created.ToString(),
            Message = "User created successfully",
            Data = user
        });
    }
    
    // 🔹 UPDATE
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, UpdateUserDto userDto)
    {
        var user = await _userService.UpdateUserAsync(id, userDto);

        if (user == null)
            return NotFound(new ResponseHandlers<UserDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "User not found"
            });

        return Ok(new ResponseHandlers<UserDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "User updated successfully",
            Data = user
        });
    }

    // 🔹 DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var deleted = await _userService.DeleteUserAsync(id);

        if (!deleted)
            return NotFound(new ResponseHandlers<object>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "User not found"
            });

        return Ok(new ResponseHandlers<object>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "User deleted successfully"
        });
    }
}