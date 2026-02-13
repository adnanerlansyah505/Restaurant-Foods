using RestaurantFoods.Commons.Pagination;
using RestaurantFoods.Dtos.Foods;
using RestaurantFoods.Repositories.Interfaces;
using RestaurantFoods.Services.Interfaces;

namespace RestaurantFoods.Services;

public class FoodService : IFoodService
{
    private readonly IFoodRepository _foodRepository;

    public FoodService(IFoodRepository foodRepository)
    {
        _foodRepository = foodRepository;
    }

    public Task<(IEnumerable<FoodDto> Foods, PaginationMeta Meta)> GetFoodsAsync(int page, int pageSize)
    {
        var emptyFoods = Enumerable.Empty<FoodDto>();

        var meta = new PaginationMeta
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = 0,
            TotalPages = 0
        };

        return Task.FromResult((emptyFoods, meta));
    }

    public Task<FoodDto?> GetFoodByIdAsync(Guid id)
    {
        return Task.FromResult<FoodDto?>(null);
    }

    public Task<FoodDto> CreateFoodAsync(SaveFoodDto foodDto)
    {
        var dummy = new FoodDto(
            Guid.NewGuid(),
            foodDto.Name,
            foodDto.Description,
            foodDto.Price,
            foodDto.Cost,
            foodDto.CategoryId
        );

        return Task.FromResult(dummy);
    }

    public Task<FoodDto?> UpdateFoodAsync(Guid id, SaveFoodDto foodDto)
    {
        var updated = new FoodDto(
            id,
            foodDto.Name,
            foodDto.Description,
            foodDto.Price,
            foodDto.Cost,
            foodDto.CategoryId
        );

        return Task.FromResult<FoodDto?>(updated);
    }

    public Task<bool> DeleteFoodAsync(Guid id)
    {
        return Task.FromResult(false);
    }
}
