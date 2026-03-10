using System.ComponentModel.DataAnnotations.Schema;
using RestaurantFoods.Models;

namespace RestaurantFoods.Models.Data;

[Table("categories")]
public class Category : BaseEntity
{
    [Column("name", TypeName = "nvarchar(255)")]
    public required string Name { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    // Navigation property
    public ICollection<Food> Foods { get; set; } = new List<Food>();
}