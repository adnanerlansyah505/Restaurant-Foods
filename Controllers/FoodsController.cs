using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore;
using RestaurantFoods.Services;
using RestaurantFoods.Services.Interfaces;
using RestaurantFoods.Models;
using RestaurantFoods.Models.Data;
using RestaurantFoods.Dtos.Foods;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantFoods.Controllers;

[Authorize(Roles = "admin")]
[ApiController]
[Route("api/foods")]
public class FoodsController : BaseApiController
{
    private readonly IFoodService _foodService;

    public FoodsController(IFoodService foodService)
    {
        _foodService = foodService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
    )
    {
        var (foods, meta) = await _foodService.GetFoodsAsync(page, pageSize);

        if (foods == null) 
            return NotFoundResponse<IEnumerable<FoodDto>>("Foods not found");

        return SuccessResponse(foods, "Foods retrieved successfully", meta);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFoodById(Guid id)
    {
        var food = await _foodService.GetFoodByIdAsync(id);

        if (food == null)
            return NotFoundResponse<IEnumerable<FoodDto>>("Foods not found");

        return SuccessResponse(food, "Food retrieved successfully");
    }

    [HttpPost]
    public async Task<IActionResult> CreateFood(SaveFoodDto foodDto)
    {
        var food = await _foodService.CreateFoodAsync(foodDto);

        return CreatedResponse(food, "Food created successfully");
    }

    // [HttpPut("{id}")]
    // public async Task<IActionResult> UpdateFood(Guid id, SaveFoodDto foodDto)
    // {

    // }

}