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

    [HttpPost("pay")]
    public async Task<IActionResult> CreatePayment(CreatePaymentDto dto)
    {
        var snapToken = await _paymentService.CreateTransactionAsync(dto.OrderId);

        return SuccessResponse(new { token = snapToken }, "Pay the order successfully");
    }

}