using System.ComponentModel.DataAnnotations;

namespace RestaurantFoods.Dtos.Users;

public record UpdateUserDto(
    [Required]
    [StringLength(50)]
    string Name,
    
    [EmailAddress]
    string Email,

    [Required]
    string Username,

    [Required, MinLength(8)]
    string Password,

    Guid RoleId
);