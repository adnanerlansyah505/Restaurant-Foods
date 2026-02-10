using System.ComponentModel.DataAnnotations;

namespace RestaurantFoods.Dtos.Users;

public record CreateUserDto(
    [Required]
    [StringLength(50)]
    string Name,
    
    [Required]
    [EmailAddress]
    string Email
);