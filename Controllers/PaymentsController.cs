using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore;
using RestaurantFoods.Data;
using RestaurantFoods.Models;
using RestaurantFoods.Services;
using RestaurantFoods.Utilities.Handlers;
using RestaurantFoods.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantFoods.Controllers;

[Authorize(Roles = "user")]
[ApiController]
[Route("api/payments")]
public class PaymentsController : BaseApiController
{

}