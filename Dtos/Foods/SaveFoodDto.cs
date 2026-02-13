using System.ComponentModel.DataAnnotations;

namespace RestaurantFoods.Dtos.Foods;

public record SaveFoodDto(
    [Required]
    [StringLength(50)]
    string Name,
    
    string Description,

    [Required]
    int Price,

    [Required]
    int Cost,
    
    Guid? CategoryId = null
);