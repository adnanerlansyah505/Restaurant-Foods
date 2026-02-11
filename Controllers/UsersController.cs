using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore;
using RestaurantFoods.Data;
using RestaurantFoods.Models;
using RestaurantFoods.Services;
using RestaurantFoods.Utilities.Handlers;
using RestaurantFoods.Dtos.Users;
using RestaurantFoods.Services.Interfaces;

namespace RestaurantFoods.Controllers;

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
                Message = "Users not found"
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
}