using Microsoft.EntityFrameworkCore;
using RestaurantFoods.Commons.Pagination;
using RestaurantFoods.Dtos.Foods;
using RestaurantFoods.Repositories.Interfaces;
using RestaurantFoods.Services.Interfaces;
using RestaurantFoods.Models.Data;
using RestaurantFoods.Services.Security;

namespace RestaurantFoods.Services;

public class FoodService : IFoodService
{
    private readonly IFoodRepository _foodRepository;

    public FoodService(IFoodRepository foodRepository)
    {
        _foodRepository = foodRepository;
    }

    public async Task<(IEnumerable<FoodDto> Foods, PaginationMeta Meta)> GetFoodsAsync(int page, int pageSize)
    {
        page = page <= 0 ? 1 : page;
        pageSize = pageSize <= 0 || pageSize > 10 ? 10 : pageSize;

        var query = _foodRepository.Query();

        var totalItems = await query.CountAsync();

        var foods = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(f => new FoodDto(
                    f.Guid,
                    f.Name,
                    f.Description,
                    f.Price,
                    f.Cost,
                    f.CategoryId
                ))
                .ToListAsync();

        var meta = new PaginationMeta
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
        };

        return (foods, meta);
    }

    public async Task<FoodDto?> GetFoodByIdAsync(Guid id)
    {
        var food = await _foodRepository.GetByIdAsync(id);

        if (food == null)
            return null;

        return new FoodDto(
            food.Guid,
            food.Name,
            food.Description,
            food.Price,
            food.Cost,
            food.CategoryId
        );
    }

    public async Task<FoodDto> CreateFoodAsync(SaveFoodDto foodDto)
    {
        var food = new Food
        {
            Name = foodDto.Name,
            Description = foodDto.Description,
            Price = foodDto.Price,
            Cost = foodDto.Cost,
            CategoryId = foodDto.CategoryId
        };

        await _foodRepository.AddAsync(food);
        await _foodRepository.SaveChangesAsync();

        return new FoodDto(
            food.Guid,
            food.Name,
            food.Description,
            food.Price,
            food.Cost,
            food.CategoryId
        );
    }

    public async Task<FoodDto?> UpdateFoodAsync(Guid id, SaveFoodDto foodDto)
    {
        var food = await _foodRepository.GetByIdAsync(id);
        if (food == null) return null;

        food.Name = foodDto.Name;
        food.Description = foodDto.Description;
        food.Price = foodDto.Price;
        food.Cost = foodDto.Cost;
        food.CategoryId = foodDto.CategoryId;

        await _foodRepository.SaveChangesAsync();

        return new FoodDto(
            food.Guid,
            food.Name,
            food.Description,
            food.Price,
            food.Cost,
            food.CategoryId
        );
    }

    public async Task<bool> DeleteFoodAsync(Guid id)
    {
        var food = await _foodRepository.GetByIdAsync(id);
        if (food == null) return false;

        await _foodRepository.DeleteAsync(food);
        await _foodRepository.SaveChangesAsync();
        return true;
    }
}
