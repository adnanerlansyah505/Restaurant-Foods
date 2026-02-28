using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore;
using RestaurantFoods.Data;
using RestaurantFoods.Models;
using RestaurantFoods.Services;
using RestaurantFoods.Utilities.Handlers;
using RestaurantFoods.Services.Interfaces;
using RestaurantFoods.Dtos.Payments;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RestaurantFoods.Controllers;

[Authorize(Roles = "user")]
[ApiController]
[Route("api/payments")]
public class PaymentsController : BaseApiController
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePayment(CreatePaymentDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            return Unauthorized("User ID claim not found");

        var userId = Guid.Parse(userIdClaim.Value);

        var snapToken = await _paymentService.CreateTransactionAsync(dto.OrderId, userId);

        return SuccessResponse(new { token = snapToken }, "Pay the order successfully");
    }

    [AllowAnonymous]
    [HttpPost("notification")]
    public async Task<IActionResult> MidtransNotification()
    {
        using var reader = new StreamReader(Request.Body);
        var json = await reader.ReadToEndAsync();

        await _paymentService.HandleNotificationAsync(json);

        return Ok();
    }

}