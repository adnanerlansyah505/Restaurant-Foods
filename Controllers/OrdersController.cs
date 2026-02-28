using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore;
using RestaurantFoods.Data;
using RestaurantFoods.Models;
using RestaurantFoods.Services;
using RestaurantFoods.Utilities.Handlers;
using RestaurantFoods.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using RestaurantFoods.Dtos.Orders;
using System.Security.Claims;

namespace RestaurantFoods.Controllers;

[Authorize(Roles = "user")]
[ApiController]
[Route("api/orders")]
public class OrdersController : BaseApiController
{
    private readonly IOrderService _orderService;
    private readonly IUserService _userService;

    public OrdersController(IOrderService orderService, IUserService userService)
    {
        _orderService = orderService;
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            return Unauthorized("User ID claim not found");

        var userId = Guid.Parse(userIdClaim.Value); // Error from this line

        var result = await _orderService.CreateOrderAsync(userId, dto);

        return SuccessResponse(result, "Order created successfully");
    }
}